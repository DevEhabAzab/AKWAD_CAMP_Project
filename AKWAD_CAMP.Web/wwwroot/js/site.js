function Template(data, idColumn, controllerName) {
    return `
       <div class="d-flex justify-content-between  align-items-center ">
       <a class="btn btn-default" href="/${controllerName}/Details/${data[idColumn]}"><i class="fa fa-info-circle text-primary" aria-hidden="true"></i></a>
                <a class="btn btn-default" href="/${controllerName}/Edit/${data[idColumn]}"><i class="fa fa-edit text-warning" aria-hidden="true"></i></a>
                <a class="btn btn-default" href="/${controllerName}/Delete/${data[idColumn]}"><i class="fa fa-trash text-danger" aria-hidden="true"></i></a>
                </div>`
}

function listCell(data) {
    var ul = `<table class="table">
                <tr>
                    <th scope="col">#</th>
                    <th scope="col">${wordItemName}</th>
                    <th scope="col">${wordQuantity}</th>
                    <th scope="col">${wordUnitPrice}</th>
                </tr>`;
    data.Items.forEach(( elm,index) => {
        ul +=`<tr>
                    <th scope="row">${index+1}</th>
                    <td >${elm["itemName"]}</td>
                    <td >${elm["itemQuantity"]}</td>
                    <td >${elm["itemUnitPrice"]}</td>
                </tr>`
    })
    ul += "</table>";
    return ul;
}


function Create(url) {
    window.location = url
}


function checkBoxTemplate(data,columnName) {
    if (data[columnName] == true)
        return `<input type ="checkbox" checked disabled />`

    return `<input type ="checkbox" disabled  />`
}
function filladdItemsModal(gridId, modalId) {
    var body = document.querySelector(`#${modalId} > .modal-dialog > .modal-content > .modal-body`);
    var clrModalBody = document.querySelector(`#clrAddItemsModal > .modal-dialog > .modal-content > .modal-body`);
    clrModalBody.innerHTML = "";
    body.innerHTML = "";
    var grid = $(`#${gridId}`).data("kendoGrid");
    var rows = grid.select();
    var fragment = new DocumentFragment();
    rows.each(function (index, row) {
        var selectedItem = grid.dataItem(row);

        var rowDiv = document.createElement("div");
        rowDiv.className = "row itemrow";
        var itemDiv = document.createElement("div");

        itemDiv.className = "col-3 control-label itemName";
        itemDiv.innerText = selectedItem.ItemName+":";
        var quantityDiv = document.createElement("div");
        quantityDiv.className = "col-3";

        var itemUnitPriceDiv = document.createElement("div");
        itemUnitPriceDiv.className = "col-3";
        itemUnitPriceDiv.innerHTML = selectedItem.ItemPrice;
        itemUnitPriceDiv.setAttribute("id", `unitprice-${selectedItem.ItemNumber}`);

        var priceDiv = document.createElement("div");
        priceDiv.innerHTML = selectedItem.ItemPrice;
        priceDiv.className = "col-3";
        priceDiv.setAttribute("id", `price-${selectedItem.ItemNumber}`);


        var quantityInput = document.createElement("input");
        quantityInput.setAttribute("type","number");
        quantityInput.setAttribute("min",1);
        quantityInput.setAttribute("onkeydown","updatePrice(this)");
        quantityInput.setAttribute("onchange","updatePrice(this)");
        quantityInput.setAttribute("oninput","updatePrice(this)");
        quantityInput.setAttribute("value",1);
        quantityInput.setAttribute("class","form-control quantity");
        quantityInput.setAttribute("id", `quantity-${selectedItem.ItemNumber}`);

        quantityDiv.appendChild(quantityInput);

        rowDiv.appendChild(itemDiv);
        rowDiv.appendChild(itemUnitPriceDiv);
        rowDiv.appendChild(quantityDiv);
        rowDiv.appendChild(priceDiv);

        fragment.appendChild(rowDiv);
    });
    body.appendChild(fragment);
  
}
function updatePrice (e) {
    
            var id = e.id;
       
            var priceId = id.replace('quantity-', 'price-');
           

            document.getElementById(priceId).innerHTML = "";
            
            var unitPrice = id.replace('quantity-', 'unitprice-');
          
    


    var quantity = e.value;
    if (quantity < 0) {
        e.value = -1 * e.value
        quantity = e.value;
    }
    if (quantity)
            document.getElementById(priceId).innerHTML = parseFloat(document.getElementById(unitPrice).innerHTML) * parseFloat(quantity);
    else    
        document.getElementById(priceId).innerHTML =0
      
}
function fillItems(e) {
    
    var itemsinputs = document.getElementById("itemsInputs");
    var lastindex = 0;
    var total = 0;
    var fragment = new DocumentFragment();
    var urlSpletted = window.location.href.split('/');
    var IsInEditView = urlSpletted[urlSpletted.length - 2].toLowerCase() == "edit";
    var controllerName = urlSpletted[urlSpletted.length - 3].toLowerCase();

    
    if (e.id == "clr") {
        itemsinputs.innerHTML = "";
    }
    else {
        var lastInput = Array.from(document.querySelectorAll(".itemnumbers")).pop();
        if (lastInput) {
            var lastInputIndex = lastInput.getAttribute("name").replace("ItemsViewModel[", "").replace("].itemNumber", "");
            lastindex = parseInt(lastInputIndex) + 1; 
        }
        var inpdiv = document.getElementById("itemsInputs");
        var hr = document.getElementById("pricehr");
        var totalprice = document.getElementById("totalPrice");
        if (totalprice) {
            total = parseFloat(totalprice.innerHTML);
            inpdiv.removeChild(hr);
            inpdiv.removeChild(totalprice.parentNode);
        }
            
    }

    var inputlength = Array.from(document.querySelectorAll(".rowinputs")).length;
    //console.log(inputlength);
    if (inputlength == 0) {
        var labelDiv = document.createElement("div");
        labelDiv.className = "row"

        var itemNameLabel = document.createElement("div");
        if (controllerName == 'supertemplates')
            itemNameLabel.innerHTML = wordName;
        else
            itemNameLabel.innerHTML = wordTemplateItemName;
        itemNameLabel.className = "col-3 control-label text-center";

        var itemNumberLabel = document.createElement("div");
        if (controllerName == 'supertemplates')
            itemNameLabel.innerHTML = wordTemplateItemId;
        else
            itemNumberLabel.innerHTML = wordNumber;
        itemNumberLabel.className = "col-2 control-label text-center";

        var itemQuantityLabel = document.createElement("div");
        itemQuantityLabel.innerHTML = wordQuantity;
        itemQuantityLabel.className = "col-3 control-label text-center";

        var itemQuantityPriceLabel = document.createElement("div");
        itemQuantityPriceLabel.innerHTML = wordPrice;
        itemQuantityPriceLabel.className = "col-3 control-label text-center";
        
        var deleteDummyDiv = document.createElement("div");
        deleteDummyDiv.className = "col-1";



        labelDiv.appendChild(itemNameLabel);
        labelDiv.appendChild(itemNumberLabel);
        labelDiv.appendChild(itemQuantityLabel);
        labelDiv.appendChild(itemQuantityPriceLabel);
        labelDiv.appendChild(deleteDummyDiv);

        fragment.appendChild(labelDiv);
    }
    
    document.querySelectorAll(`.itemrow`).forEach((value, index) => {

        

        var itemName = value.querySelector(".itemName").innerHTML.replace(":", "");
        
        var quantity = value.querySelector(".quantity").value;
        var itemNumber = value.querySelector(".quantity").id.replace("quantity-", "");
        var itemPrice = parseFloat(value.querySelector(`#unitprice-${itemNumber}`).innerHTML);
        if (quantity) {
            total += itemPrice * parseFloat(quantity);
            var s = document.getElementById(`itemNumberInput-${itemNumber}`);
            if (s) {
                var parent = s.parentNode;
                var q = parent.querySelector(".itemQuantity").value;
                parent.querySelector(".quantityPrice").value = (parseFloat(q) + parseFloat(quantity)) * itemPrice;
                parent.querySelector(".itemQuantity").value = (parseFloat(q) + parseFloat(quantity));
                //console.log("quantity", q);
                /*var totalpriceDiv = document.getElementById("totalPrice");
                var totPrice = parseFloat(totalpriceDiv.value) + (parseFloat(quantity) * itemPrice);
                totalpriceDiv.innerHTML = "Total: " + totPrice;*/
            } else {


                var inpdiv = document.createElement("div");
                inpdiv.className = "row rowinputs mt-1";

                var itemNameInp = document.createElement("input");
                itemNameInp.setAttribute("type", "text");
                itemNameInp.setAttribute("value", itemName);
                itemNameInp.setAttribute("name", `ItemsViewModel[${inputlength + index}].itemName`);
                itemNameInp.setAttribute("readonly", true);
                itemNameInp.className = "col-3 form-control";

                var itemInp = document.createElement("input");
                itemInp.setAttribute("type", "text");
                //itemInp.setAttribute("class", "itemnumbers");
                itemInp.setAttribute("value", itemNumber);
                itemInp.id = `itemNumberInput-${itemNumber}`
                itemInp.setAttribute("name", `ItemsViewModel[${inputlength + index}].itemNumber`);
                itemInp.setAttribute("readonly", true);
                itemInp.className = "col-2 form-control itemnumbers";


                var quantityInp = document.createElement("input");
                quantityInp.setAttribute("type", "number");
                quantityInp.setAttribute("value", parseFloat(quantity));
                quantityInp.setAttribute("name", `ItemsViewModel[${inputlength + index}].itemQuantity`);
                quantityInp.setAttribute("onkeydown", "updatePriceInUpdate(this)");
                quantityInp.setAttribute("onchange", "updatePriceInUpdate(this)");
                quantityInp.setAttribute("oninput", "updatePriceInUpdate(this)");
                //if (!IsInEditView) {
                //    quantityInp.setAttribute("readonly", true);

                //}
                quantityInp.className = "col-3 form-control itemQuantity";

                var quantityPrice = document.createElement("input");
                quantityPrice.setAttribute("type", "text");
                quantityPrice.setAttribute("value", itemPrice * parseFloat(quantity));
                quantityPrice.setAttribute("readonly", true);
                quantityPrice.setAttribute("name", `ItemsViewModel[${inputlength + index}].quantityPrice`);
                quantityPrice.className = "col-3 form-control quantityPrice";

                var unitPriceInput = document.createElement("input");
                unitPriceInput.setAttribute("value", itemPrice);
                unitPriceInput.setAttribute("hidden", "");
                unitPriceInput.className = "unitPrice";

                var deleteIcon = document.createElement("button");
                deleteIcon.innerHTML = `<i class="fa fa-trash"></i>`;
                deleteIcon.setAttribute("type", "button");
                deleteIcon.setAttribute("onclick", "delteRow(this)");
                deleteIcon.className = "col-1"

                inpdiv.appendChild(itemNameInp);
                inpdiv.appendChild(itemInp);
                inpdiv.appendChild(quantityInp);
                inpdiv.appendChild(quantityPrice);
                inpdiv.appendChild(unitPriceInput);
                inpdiv.appendChild(deleteIcon);
                fragment.appendChild(inpdiv);
            }
        }
        
    });
    var totalRow = document.createElement("div");
    totalRow.className = "row";
    var totalPriceWord = document.createElement("h5");
    totalPriceWord.className = "offset-9 col-3";
    totalPriceWord.innerHTML = wordTotal;
    totalRow.appendChild(totalPriceWord);

    var totalPrice = document.createElement("h5");
    totalPrice.className = "offset-9 col-3";
    totalPrice.id = "totalPrice";
    totalPrice.innerHTML =  total;
    totalRow.appendChild(totalPrice);
    var hr = document.createElement("hr");
    hr.id = "pricehr"
    fragment.appendChild(hr);
    fragment.appendChild(totalRow);
    itemsinputs.appendChild(fragment);
    var clrbody = document.querySelector(`#addItemsModal > .modal-dialog > .modal-content > .modal-body`);
    clrbody.innerHTML = "";
    var clrAddbody = document.querySelector(`#clrAddItemsModal > .modal-dialog > .modal-content > .modal-body`);
    clrAddbody.innerHTML = "";
    
}
function delteRow(e) {
    //debugger;
    var parent = e.parentNode;
    var grandParent = parent.parentNode;
    var deletedprice = parent.querySelector(".quantityPrice").value;
    var totalPrice = document.getElementById("totalPrice").innerHTML;
    var newPrice = parseFloat(totalPrice) - parseFloat(deletedprice);

    document.getElementById("totalPrice").innerHTML =  newPrice
    grandParent.removeChild(parent);
    var inputlength = Array.from(document.querySelectorAll(".rowinputs")).length;
    ////console.log(inputlength);
    ////console.log(parent.querySelector(".itemnumbers"),"before");
    var indexOfDeletedRow = parseInt(parent.querySelector(".itemnumbers").name.replace("]", "[").split("[")[1]);
    ////console.log("after");
    for (var i = indexOfDeletedRow; i < inputlength; i++) {
        grandParent.querySelector(`input[name="ItemsViewModel[${i + 1}].itemName"]`).name = `ItemsViewModel[${i}].itemName`;
        grandParent.querySelector(`input[name="ItemsViewModel[${i + 1}].itemNumber"]`).name = `ItemsViewModel[${i}].itemNumber`;
        grandParent.querySelector(`input[name="ItemsViewModel[${i + 1}].itemQuantity"]`).name = `ItemsViewModel[${i}].itemQuantity`;
        grandParent.querySelector(`input[name="ItemsViewModel[${i + 1}].QuantityPrice"]`).name = `ItemsViewModel[${i}].QuantityPrice`;

    }
    ////console.log(indexOfDeletedRow);

    if (inputlength == 0) {
        document.getElementById("itemsInputs").innerHTML = "";
    }

}



$('document').ready(function () {
    var grid = document.getElementById('grid');
    if (grid) {
       
        var beforeGrid = grid.previousElementSibling;
        var tagName = beforeGrid.tagName;
        ////console.log(tagName);
        
        if (tagName.toLowerCase() == "p") {
            beforeGrid.innerHTML = "";
        }
    }

    
   

    var urlSpletted = window.location.href.split('/');
    var IsInEditProductDetailsView = urlSpletted[urlSpletted.length - 2].toLowerCase() == "edit" && urlSpletted[urlSpletted.length - 3].toLowerCase() == "productdetail";
    var IsInEditTemplatesView = urlSpletted[urlSpletted.length - 2].toLowerCase() == "edit" && urlSpletted[urlSpletted.length - 3].toLowerCase() == "templates";
    var IsInEditSuperTemplatesView = urlSpletted[urlSpletted.length - 2].toLowerCase() == "edit" && urlSpletted[urlSpletted.length - 3].toLowerCase() == "supertemplates";
    if (IsInEditProductDetailsView || IsInEditTemplatesView || IsInEditSuperTemplatesView) {
        document.querySelectorAll(`.rowinputs`).forEach((value, index) => {
            value.querySelector(".itemQuantity").setAttribute("onkeydown", "updatePriceInUpdate(this)");
            value.querySelector(".itemQuantity").setAttribute("onchange", "updatePriceInUpdate(this)");
            value.querySelector(".itemQuantity").setAttribute("oninput", "updatePriceInUpdate(this)");
        });
        //console.log("ready");
    }
    ////console.log(getCalture());
});
function updatePriceInUpdate(e) {
    var parent = e.parentNode;
    var quantity = parent.querySelector(".itemQuantity").value;
    var unitPrice = parent.querySelector(".unitPrice").value;
    parent.querySelector(".quantityPrice").value = quantity * unitPrice;
    var grandParent = parent.parentNode;
    var total = 0;
    document.querySelectorAll(`.rowinputs`).forEach((value, index) => {
        total += parseFloat(value.querySelector(".quantityPrice").value);
    });
    document.getElementById("totalPrice").innerHTML =  total


}
function templatesAndProductsDupOnChange() {
    var grid = $(`#grid`).data("kendoGrid");
    var rows = Array.from(grid.select());

    if (rows.length > 0) {
        document.getElementById("dupBtn").removeAttribute("hidden");

    } else {
        document.getElementById("dupBtn").setAttribute("hidden","");
    }

}

async function duplicateTemp(url) {
    var grid = $(`#grid`).data("kendoGrid");
    var arr = Array.from(grid.select());
   
    var elm = grid.dataItem(arr[0]) ;
    ////console.log(elm);
    var body = {
        id: elm.TemplateId
    }
    var dupres = await postData(url, body);

    window.location.reload();
  

   

}
function OnChange (e) {
    var grid = $(`#grid`).data("kendoGrid");
    var rows = Array.from(grid.select());
    
    if (rows.length > 0) {
        document.getElementById("addItemsBtn").removeAttribute("hidden");
        

        var inputlength = Array.from(document.querySelectorAll(".rowinputs")).length;
        if (inputlength>0) {
            document.getElementById("clrAddItemsBtn").removeAttribute("hidden");

        } else {
            document.getElementById("clrAddItemsBtn").setAttribute("hidden", "");

        }
    } else {
        document.getElementById("addItemsBtn").setAttribute("hidden", "");
        document.getElementById("clrAddItemsBtn").setAttribute("hidden", "");


    }
}

function exportExcel() {

    var grid = $(`#grid`).data("kendoGrid");
    var rows = Array.from(grid.select());
    if (rows.length == 1) {
        var base_url = window.location.origin;
        const myUrlWithParams = new URL(`${base_url}/Home/DownloadExcelEPPlus`);

        rows.forEach((elm) => {
            var item = grid.dataItem(elm)
            myUrlWithParams.searchParams.append("id", item.TemplateId);

        });
        window.location = myUrlWithParams.href;

    }
    
   
    
}



function getCalture() {
    var current = document.cookie.split('; ').reduce((r, v) => {
        const parts = v.split('=')
        return parts[0] === '.AspNetCore.Culture' ? decodeURIComponent(parts[1]) : r
    }, '').split('|')[0].split('=')[1]
    return current;
}


function list_to_tree(list) {
    var map = {}, node, roots = [], i;

    for (i = 0; i < list.length; i += 1) {
        map[list[i].id] = i; // initialize the map
        list[i].children = []; // initialize the children
    }

    for (i = 0; i < list.length; i += 1) {
        node = list[i];
        if (node.parentId !== "0") {
            // if you have dangling branches check that map[node.parentId] exists
            list[map[node.parentId]].children.push(node);
        } else {
            roots.push(node);
        }
    }
    return roots;
}


var DPform = document.getElementById("DPform");
if (DPform) {
    DPform.onsubmit = () => {
        var profit = document.getElementById("Profit");
        var damage = document.getElementById("Damage");

        if (profit.value.includes(",")) {
            profit.value = profit.value.replace(",",".")
        }

        if (damage.value.includes(",")) {
            damage.value = damage.value.replace(",", ".")
        }

        DPform.submit();
    }
}


function curruncylan(data, cult) {
    if (cult.startsWith("en")) {
        var us = Intl.NumberFormat('en-US')
        return us.format(data) + " L.E";
    }
    else if (cult.startsWith("ar")) {
        var us = Intl.NumberFormat('ar-EG')
        return us.format(data) + " ج.م";
       

    }
}


async function postData(url = '', data) {
    ////console.log(data);
    // Default options are marked with *
    const response = await fetch(url, {
        method: 'POST', // *GET, POST, PUT, DELETE, etc.
        credentials: 'same-origin', // include, *same-origin, omit
        headers: {
            'Accept': 'application/json; charset=utf-8',
            'Content-Type': 'application/json;charset=UTF-8'
        },
      
        body: JSON.stringify(data)// body data type must match "Content-Type" header
    });
    return response.json(); // parses JSON response into native JavaScript objects
}

function filladdTemplatesItemsModal(gridId, modalId) {
    var body = document.querySelector(`#${modalId} > .modal-dialog > .modal-content > .modal-body`);
    var clrModalBody = document.querySelector(`#clrAddItemsModal > .modal-dialog > .modal-content > .modal-body`);
    clrModalBody.innerHTML = "";
    body.innerHTML = "";
    var grid = $(`#${gridId}`).data("kendoGrid");
    var rows = grid.select();
    var fragment = new DocumentFragment();
    rows.each(function (index, row) {
        var selectedItem = grid.dataItem(row);

        var rowDiv = document.createElement("div");
        rowDiv.className = "row itemrow";
        var itemDiv = document.createElement("div");

        itemDiv.className = "col-3 control-label itemName";
        itemDiv.innerText = selectedItem.TemplateName + ":";
        var quantityDiv = document.createElement("div");
        quantityDiv.className = "col-3";

        var itemUnitPriceDiv = document.createElement("div");
        itemUnitPriceDiv.className = "col-3";
        itemUnitPriceDiv.innerHTML = selectedItem.TotalPrice;
        itemUnitPriceDiv.setAttribute("id", `unitprice-${selectedItem.TemplateId}`);

        var priceDiv = document.createElement("div");
        priceDiv.innerHTML = selectedItem.TotalPrice;
        priceDiv.className = "col-3";
        priceDiv.setAttribute("id", `price-${selectedItem.TemplateId}`);


        var quantityInput = document.createElement("input");
        quantityInput.setAttribute("type", "number");
        quantityInput.setAttribute("min", 1);
        quantityInput.setAttribute("onkeydown", "updatePrice(this)");
        quantityInput.setAttribute("onchange", "updatePrice(this)");
        quantityInput.setAttribute("oninput", "updatePrice(this)");
        quantityInput.setAttribute("value", 1);
        quantityInput.setAttribute("class", "form-control quantity");
        quantityInput.setAttribute("id", `quantity-${selectedItem.TemplateId}`);

        quantityDiv.appendChild(quantityInput);

        rowDiv.appendChild(itemDiv);
        rowDiv.appendChild(itemUnitPriceDiv);
        rowDiv.appendChild(quantityDiv);
        rowDiv.appendChild(priceDiv);

        fragment.appendChild(rowDiv);
    });
    body.appendChild(fragment);

}