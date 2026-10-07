const state={structures:[],employees:[],inspections:[]};
const pages={
dashboard:["Сводка","Состояние защитных сооружений и распределения персонала"],
structures:["Защитные сооружения","Вместимость, состояние и ответственные лица"],
employees:["Сотрудники","Распределение сотрудников по защитным сооружениям"],
inspections:["Проверки","История проверок и контроль состояния"]
};
const entityModal=new bootstrap.Modal(document.getElementById("entityModal"));
const detailsModal=new bootstrap.Modal(document.getElementById("detailsModal"));
const form=document.getElementById("entityForm");
const refreshButton=document.getElementById("refreshButton");
const employeeStructureFilter=document.getElementById("employeeStructureFilter");
const employeeAssignmentFilter=document.getElementById("employeeAssignmentFilter");
const inspectionStructureFilter=document.getElementById("inspectionStructureFilter");

document.querySelectorAll("[data-page]").forEach(b=>b.addEventListener("click",()=>showPage(b.dataset.page)));
refreshButton.addEventListener("click",()=>loadAll(true));
document.addEventListener("click",handleAction);
form.addEventListener("submit",submitModal);
employeeStructureFilter.addEventListener("change",renderEmployees);
employeeAssignmentFilter.addEventListener("change",renderEmployees);
inspectionStructureFilter.addEventListener("change",renderInspections);

async function api(url,options={}){
  const response=await fetch(url,{headers:{"Content-Type":"application/json",...(options.headers||{})},...options});
  if(response.status===204)return null;
  const data=await response.json().catch(()=>null);
  if(!response.ok)throw new Error(data?.detail||data?.title||"Ошибка запроса");
  return data;
}

function showPage(name){
  document.querySelectorAll(".page").forEach(x=>x.classList.remove("active"));
  document.querySelectorAll("[data-page]").forEach(x=>x.classList.toggle("active",x.dataset.page===name));
  document.getElementById("page-"+name).classList.add("active");
  document.getElementById("pageTitle").textContent=pages[name][0];
  document.getElementById("pageSubtitle").textContent=pages[name][1];
}

async function loadAll(showFeedback=false){
  if(showFeedback){refreshButton.disabled=true;refreshButton.textContent="Обновление...";}
  try{
    const [dashboard,structures,employees,inspections]=await Promise.all([
      api("/api/dashboard"),api("/api/protective-structures"),api("/api/employees"),api("/api/inspections")
    ]);
    Object.assign(state,{structures,employees,inspections});
    populateFilters();renderDashboard(dashboard);renderStructures();renderEmployees();renderInspections();
    if(showFeedback)toast("Данные обновлены");
  }catch(error){toast(error.message,true);}
  finally{if(showFeedback){refreshButton.disabled=false;refreshButton.textContent="Обновить";}}
}

function renderDashboard(d){
  const metrics=[["Сооружения",d.protectiveStructureCount],["Общая вместимость",d.totalCapacity],["Распределено",d.assignedEmployeeCount],["Свободно мест",d.availableCapacity]];
  document.getElementById("metrics").innerHTML=metrics.map(([l,v])=>'<div class="col-sm-6 col-xl-3"><div class="metric-card"><div class="metric-label">'+l+'</div><div class="metric-value">'+v+'</div></div></div>').join("");
  document.getElementById("conditionSummary").innerHTML=[
    statusRow("Готовы",d.readyCount,"success"),statusRow("Требуют внимания",d.requiresAttentionCount,"warning"),statusRow("Не готовы",d.notReadyCount,"danger")
  ].join("");
  document.getElementById("distributionSummary").innerHTML=[
    statusRow("Не распределено сотрудников",d.unassignedEmployeeCount,"secondary"),
    statusRow("Просроченные проверки",d.overdueInspectionCount,d.overdueInspectionCount?"danger":"success")
  ].join("");
}

function statusRow(label,value,color){return '<div class="status-row"><span>'+label+'</span><span class="badge text-bg-'+color+' badge-status">'+value+'</span></div>';}
function conditionLabel(v){return({Ready:"Готово",RequiresAttention:"Требует внимания",NotReady:"Не готово"})[v]||v;}
function conditionBadge(v){const c={Ready:"success",RequiresAttention:"warning",NotReady:"danger"}[v]||"secondary";return '<span class="badge text-bg-'+c+' badge-status">'+conditionLabel(v)+'</span>';}
function typeLabel(v){return({Shelter:"Убежище",AntiRadiationShelter:"ПРУ",SimpleCover:"Укрытие"})[v]||v;}
function structureName(id){return state.structures.find(x=>x.id===id)?.name||"Не распределён";}

function renderStructures(){
  const body=document.getElementById("structuresTable");
  body.innerHTML=state.structures.length?state.structures.map(x=>`
  <tr>
    <td>${escapeHtml(x.registrationNumber)}</td>
    <td><strong>${escapeHtml(x.name)}</strong><div class="small text-secondary">${escapeHtml(x.address)}</div></td>
    <td>${typeLabel(x.type)}</td><td>${x.capacity}</td><td>${x.assignedEmployeeCount}</td><td>${x.availableCapacity}</td>
    <td>${conditionBadge(x.condition)}</td>
    <td><div class="action-buttons">
      <button class="btn btn-sm btn-outline-secondary" data-action="details-structure" data-id="${x.id}">Карточка</button>
      <button class="btn btn-sm btn-outline-primary" data-action="edit-structure" data-id="${x.id}">Изменить</button>
      <button class="btn btn-sm btn-outline-danger" data-action="delete-structure" data-id="${x.id}">Удалить</button>
    </div></td>
  </tr>`).join(""):emptyRow(8);
}

function renderEmployees(){
  const sid=Number(employeeStructureFilter.value)||null;
  const assignment=employeeAssignmentFilter.value;
  const items=state.employees.filter(x=>{
    if(sid&&x.protectiveStructureId!==sid)return false;
    if(assignment==="assigned"&&!x.protectiveStructureId)return false;
    if(assignment==="unassigned"&&x.protectiveStructureId)return false;
    return true;
  });
  document.getElementById("employeesTable").innerHTML=items.length?items.map(x=>`
  <tr>
    <td>${escapeHtml(x.personnelNumber)}</td><td><strong>${escapeHtml(x.fullName)}</strong></td>
    <td>${escapeHtml(x.department)}</td><td>${escapeHtml(x.position)}</td>
    <td>${escapeHtml(x.protectiveStructureId?structureName(x.protectiveStructureId):"Не распределён")}</td>
    <td><div class="action-buttons">
      <button class="btn btn-sm btn-outline-secondary" data-action="assign-employee" data-id="${x.id}">Распределить</button>
      <button class="btn btn-sm btn-outline-primary" data-action="edit-employee" data-id="${x.id}">Изменить</button>
      <button class="btn btn-sm btn-outline-danger" data-action="delete-employee" data-id="${x.id}">Удалить</button>
    </div></td>
  </tr>`).join(""):emptyRow(6);
}

function renderInspections(){
  const sid=Number(inspectionStructureFilter.value)||null;
  const items=state.inspections.filter(x=>!sid||x.protectiveStructureId===sid);
  document.getElementById("inspectionsTable").innerHTML=items.length?items.map(x=>`
  <tr>
    <td>${formatDate(x.inspectionDate)}</td><td>${escapeHtml(structureName(x.protectiveStructureId))}</td>
    <td>${escapeHtml(x.inspectorName)}</td><td>${conditionBadge(x.resultCondition)}</td>
    <td>${x.nextInspectionDate?formatDate(x.nextInspectionDate):"—"}</td><td>${escapeHtml(x.findings||"—")}</td>
  </tr>`).join(""):emptyRow(6);
}

function populateFilters(){
  populateStructureFilter(employeeStructureFilter,"Все сооружения");
  populateStructureFilter(inspectionStructureFilter,"Все сооружения");
}
function populateStructureFilter(el,label){
  const selected=el.value;
  el.innerHTML='<option value="">'+label+'</option>'+state.structures.map(x=>'<option value="'+x.id+'">'+escapeHtml(x.name)+'</option>').join("");
  if([...el.options].some(x=>x.value===selected))el.value=selected;
}

function handleAction(event){
  const button=event.target.closest("[data-action]");if(!button)return;
  const action=button.dataset.action,id=Number(button.dataset.id);
  if(action==="details-structure")return showStructureDetails(id);
  if(action==="assign-employee")return openAssignment(id);
  if(action.startsWith("create-"))return openEditor(action.slice(7));
  if(action.startsWith("edit-"))return openEditor(action.slice(5),id);
  if(action.startsWith("delete-"))return removeEntity(action.slice(7),id);
}

function openEditor(type,id=null){
  const collection=type==="structure"?"structures":type==="employee"?"employees":"inspections";
  const entity=id?state[collection].find(x=>x.id===id):{};
  form.dataset.type=type;form.dataset.id=id||"";
  document.getElementById("modalTitle").textContent=(id?"Изменить: ":"Добавить: ")+entityTitle(type);
  document.getElementById("modalBody").innerHTML=editorFields(type,entity||{});
  entityModal.show();
}

function openAssignment(id){
  const employee=state.employees.find(x=>x.id===id);
  form.dataset.type="assignment";form.dataset.id=id;
  document.getElementById("modalTitle").textContent="Распределение: "+employee.fullName;
  document.getElementById("modalBody").innerHTML=`
  <div class="mb-3"><label class="form-label">Защитное сооружение</label>
    <select class="form-select" name="protectiveStructureId">
      <option value="">Не распределён</option>
      ${state.structures.map(x=>`<option value="${x.id}" ${x.id===employee.protectiveStructureId?"selected":""}>${escapeHtml(x.name)} — свободно ${x.availableCapacity}</option>`).join("")}
    </select>
  </div>
  <div class="form-text">Неготовое или заполненное сооружение будет отклонено системой.</div>`;
  entityModal.show();
}

function entityTitle(type){return({structure:"защитное сооружение",employee:"сотрудника",inspection:"проверку"})[type];}

function editorFields(type,x={}){
  if(type==="structure")return`
    ${input("registrationNumber","Регистрационный номер",x.registrationNumber,true)}
    ${input("name","Название",x.name,true)}
    ${select("type","Тип",["Shelter","AntiRadiationShelter","SimpleCover"],x.type,true,typeLabel)}
    ${input("address","Адрес",x.address,true)}
    ${input("capacity","Вместимость",x.capacity||1,true,"number",1)}
    ${x.id?select("condition","Состояние",["Ready","RequiresAttention","NotReady"],x.condition,true,conditionLabel):""}
    ${input("responsiblePerson","Ответственное лицо",x.responsiblePerson)}
    ${input("phone","Телефон",x.phone)}
    ${textarea("notes","Примечания",x.notes)}`;

  if(type==="employee")return`
    ${input("personnelNumber","Табельный номер",x.personnelNumber,true)}
    ${input("fullName","ФИО",x.fullName,true)}
    ${input("department","Отдел",x.department,true)}
    ${input("position","Должность",x.position,true)}`;

  return`
    ${selectStructures("protectiveStructureId","Защитное сооружение",x.protectiveStructureId,true)}
    ${input("inspectionDate","Дата проверки",x.inspectionDate||todayIso(),true,"date")}
    ${input("inspectorName","Проверяющий",x.inspectorName,true)}
    ${select("resultCondition","Результат",["Ready","RequiresAttention","NotReady"],x.resultCondition,true,conditionLabel)}
    ${textarea("findings","Замечания",x.findings)}
    ${textarea("requiredActions","Необходимые мероприятия",x.requiredActions)}
    ${input("nextInspectionDate","Следующая проверка",x.nextInspectionDate,false,"date")}`;
}

function input(name,label,value="",required=false,type="text",min=null){return`<div class="mb-3"><label class="form-label">${label}</label><input class="form-control" type="${type}" name="${name}" value="${escapeAttr(value??"")}" ${min!==null?`min="${min}"`:""} ${required?"required":""}></div>`;}
function textarea(name,label,value=""){return`<div class="mb-3"><label class="form-label">${label}</label><textarea class="form-control" name="${name}" rows="3">${escapeHtml(value||"")}</textarea></div>`;}
function select(name,label,values,selected,required,formatter){return`<div class="mb-3"><label class="form-label">${label}</label><select class="form-select" name="${name}" ${required?"required":""}><option value="">Выберите...</option>${values.map(v=>`<option value="${v}" ${v===selected?"selected":""}>${formatter(v)}</option>`).join("")}</select></div>`;}
function selectStructures(name,label,selected,required){return`<div class="mb-3"><label class="form-label">${label}</label><select class="form-select" name="${name}" ${required?"required":""}><option value="">Выберите...</option>${state.structures.map(x=>`<option value="${x.id}" ${x.id===selected?"selected":""}>${escapeHtml(x.name)}</option>`).join("")}</select></div>`;}

async function submitModal(event){
  event.preventDefault();
  const type=form.dataset.type,id=form.dataset.id;
  const data=Object.fromEntries(new FormData(form).entries());
  ["capacity","protectiveStructureId"].forEach(k=>{if(data[k]!==undefined&&data[k]!=="")data[k]=Number(data[k]);});
  Object.keys(data).forEach(k=>{if(data[k]==="")data[k]=null;});

  let endpoint,method;
  if(type==="assignment"){endpoint=`/api/employees/${id}/assignment`;method="PATCH";}
  else if(type==="structure"){endpoint=id?`/api/protective-structures/${id}`:"/api/protective-structures";method=id?"PUT":"POST";}
  else if(type==="employee"){endpoint=id?`/api/employees/${id}`:"/api/employees";method=id?"PUT":"POST";}
  else{endpoint="/api/inspections";method="POST";}

  try{await api(endpoint,{method,body:JSON.stringify(data)});entityModal.hide();toast("Изменения сохранены");await loadAll();}
  catch(error){toast(error.message,true);}
}

async function removeEntity(type,id){
  if(!confirm("Удалить запись?"))return;
  const endpoint=type==="structure"?`/api/protective-structures/${id}`:`/api/employees/${id}`;
  try{await api(endpoint,{method:"DELETE"});toast("Запись удалена");await loadAll();}
  catch(error){toast(error.message,true);}
}

async function showStructureDetails(id){
  try{
    const s=await api(`/api/protective-structures/${id}`);
    document.getElementById("detailsModalTitle").textContent=s.name;
    document.getElementById("detailsModalBody").innerHTML=`
    <div class="details-grid">
      <div class="details-item"><div class="details-label">Регистрационный номер</div><strong>${escapeHtml(s.registrationNumber)}</strong></div>
      <div class="details-item"><div class="details-label">Тип</div><strong>${typeLabel(s.type)}</strong></div>
      <div class="details-item"><div class="details-label">Состояние</div>${conditionBadge(s.condition)}</div>
      <div class="details-item"><div class="details-label">Вместимость</div><strong>${s.capacity}</strong></div>
      <div class="details-item"><div class="details-label">Распределено</div><strong>${s.assignedEmployeeCount}</strong></div>
      <div class="details-item"><div class="details-label">Свободно</div><strong>${s.availableCapacity}</strong></div>
      <div class="details-item"><div class="details-label">Ответственный</div><strong>${escapeHtml(s.responsiblePerson||"—")}</strong></div>
      <div class="details-item"><div class="details-label">Телефон</div><strong>${escapeHtml(s.phone||"—")}</strong></div>
      <div class="details-item"><div class="details-label">Адрес</div><strong>${escapeHtml(s.address)}</strong></div>
    </div>
    <div class="details-section"><h3 class="h6">Примечания</h3><p>${escapeHtml(s.notes||"Нет примечаний")}</p></div>
    <div class="details-section"><h3 class="h6">Распределённые сотрудники</h3>${simpleTable(["Табельный №","ФИО","Отдел","Должность"],s.employees.map(x=>[escapeHtml(x.personnelNumber),escapeHtml(x.fullName),escapeHtml(x.department),escapeHtml(x.position)]))}</div>
    <div class="details-section"><h3 class="h6">История проверок</h3>${simpleTable(["Дата","Проверяющий","Результат","Следующая"],s.inspections.map(x=>[formatDate(x.inspectionDate),escapeHtml(x.inspectorName),conditionBadge(x.resultCondition),x.nextInspectionDate?formatDate(x.nextInspectionDate):"—"]))}</div>`;
    detailsModal.show();
  }catch(error){toast(error.message,true);}
}

function simpleTable(headers,rows){if(!rows.length)return'<div class="text-secondary small">Нет данных</div>';return`<div class="table-responsive"><table class="table table-sm align-middle"><thead><tr>${headers.map(x=>`<th>${x}</th>`).join("")}</tr></thead><tbody>${rows.map(r=>`<tr>${r.map(c=>`<td>${c}</td>`).join("")}</tr>`).join("")}</tbody></table></div>`;}
function emptyRow(c){return`<tr><td colspan="${c}" class="empty-row">Нет данных</td></tr>`;}
function formatDate(v){if(!v)return"—";const[y,m,d]=v.split("-");return`${d}.${m}.${y}`;}
function todayIso(){const n=new Date();return new Date(n.getTime()-n.getTimezoneOffset()*60000).toISOString().slice(0,10);}
function toast(message,error=false){const el=document.createElement("div");el.className=`toast align-items-center text-bg-${error?"danger":"success"} border-0`;el.innerHTML=`<div class="d-flex"><div class="toast-body">${escapeHtml(message)}</div><button class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button></div>`;document.getElementById("toastContainer").appendChild(el);const t=new bootstrap.Toast(el,{delay:3000});t.show();el.addEventListener("hidden.bs.toast",()=>el.remove());}
function escapeHtml(v){return String(v??"").replace(/[&<>"']/g,c=>({"&":"&amp;","<":"&lt;",">":"&gt;",'"':"&quot;","'":"&#039;"}[c]));}
function escapeAttr(v){return escapeHtml(v);}
loadAll();