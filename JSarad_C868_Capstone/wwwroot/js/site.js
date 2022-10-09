// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//autoComplete user input text
function AutoComplete() {
    
    var controller = document.getElementById("completeTxt").getAttribute("controller");
    console.log(controller);
    var action = document.getElementById("completeTxt").getAttribute("action");
    console.log(action);
    var url = "/" + controller + "/" + action + "/";
    console.log(url);

    $("#completeTxt").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: url,
                type: "POST",
                dataType: "json",
                data: { "prefix": request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return item;

                    }))
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
        select: function (e, i) {
            $("#hfitem").val(i.item.val);
        },
        minLength: 0
    }).focus(function () {
        $(this).autocomplete("search");
    });
};

//hightlight selected table row


//function RowSelected(e) {
//    var controller = document.getElementById("selectable").getAttribute("controller");
//    console.log(controller);
//    var action = document.getElementById("selectable").getAttribute("action");
//    console.log(action);
//    var url = "/" + controller + "/" + action + "/";
//    console.log(url);
//    var row = document.getElementById("selectable").getAttribute("data-id");
//    console.log(row);
//    e.preventDefault();
//    var target = e.target;
//    console.log(target);
//    var id = $(target).data('id');
//    console.log(id);
//}

$((function () {
    $(".selectable").on('click', (e) => {
        e.preventDefault
        var target = e.target.parentElement;
        console.log(target);
        var id = $(target).data('id');
        console.log(id);
        var index = $(target).data('index');
        console.log(index);
        var currentRow = document.getElementById("rowIndex_" + index);
        console.log(currentRow);
        currentRow.style.backgroundColor = "yellow";
        e.target.style.backgroundColor = "yellow";
        //currentRow.childNodes.forEach.backgroundColor = "yellow";
    
    });
}()));