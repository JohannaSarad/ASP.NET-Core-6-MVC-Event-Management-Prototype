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

$((function () {
    $(".selectable").on('click', (e) => {
        e.preventDefault
        var selectedRow = e.target.parentElement;
        console.log(selectedRow);
        var id = $(selectedRow).data('id');
        console.log(id);
        var index = $(selectedRow).data('index');
        console.log(index);
        var controller = selectedRow.getAttribute("controller");
        console.log(controller);
        var action = selectedRow.getAttribute("action");
        console.log(action);
        var url = "/" + controller + "/" + action + "/" + id;
        console.log(url);

        //alternative formatting
        //var id = selectedRow.getAttribute("data-id"); 
       
        
        //first needs to check if a row has already been selected so I need to get the
        //var parent = target.parentElement;
        //console.log(parent);
        
        var trArray = selectedRow.parentElement.getElementsByTagName('tr');
        for (var row = 0; row < trArray.length; row++) {
            if (row == index)
            {
                tdArray = selectedRow.getElementsByTagName('td');
                for (var cell = 0; cell < tdArray.length; cell++)
                {
                    tdArray[cell].style.backgroundColor = "yellow";
                    //need to remove hover for these cells
                }
            }
            else
            {
                tdArray = trArray[row].getElementsByTagName('td');
                for (var cell = 0; cell < tdArray.length; cell++)
                {
                    tdArray[cell].style.backgroundColor = "#f8f9fa";
                    //need to add hover back into these cells
                   
                }
            }
        }
        $.ajax({
            url: url,
            type: "POST",
            success: function (response) {
                $(".selectionResult").val(response);
                $("#selectedId").val(id);
            }
        })
    });
}()));

$((function () {
    $(".edit").on('click', (e) => {
        e.preventDefault();
        target = e.target;
    
        id = $("#selectedId").val();
        console.log(id);
        controller = $(target).data('controller');
        action = $(target).data('action');

        pathToEdit = "/" + controller + "/" + action + "/" + id;

        if (id == 0 || id == "") {
            alert("Please Select a record to edit")
        }

        else
        {
            $.ajax({
                type: "Get",
                url: pathToEdit,
            });
        }
        
    });
}()));
                

$(function () {
    var placeholderElement = $('#PlaceHolderHere');
    var id;
    var controller;
    var action;
    $('button[data-bs-toggle="ajax-modal"]').click(function (e) {
        /* event.preventDefault();*/
        target = e.target;
        controller = $(target).data('controller');
        console.log(controller);
        action = $(target).data('action');
        console.log(action);
        var addOrEdit = $(target).data('modify-type');
        //var addOrEdit = $(this).data('modify-type'); //new
        console.log(addOrEdit);  //new
        

        if (addOrEdit == 'Edit')
        {
            id = $("#selectedId").val();
            if (id == null || id == "") {
                alert("Please Select a Record to Edit");
                return;
            }
        }
        else
        {
            id = 0;
        }
        
        var url = "/" + controller + "/" + action + "/" + id
        console.log(url);
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');

        })
    })
    placeholderElement.on('click', '[data-bs-save="modal"]', function (event) {
        event.preventDefault();
        var form = $(this).parents('.modal').find('form');
        console.log(form);
        var actionUrl = form.attr('action');
        console.log(actionUrl);
        /*var sendViewModel = JSON.serialize(form);*/
        var sendViewModel = form.serialize();
        /* var sendViewModel = JSON.stringify(serializedModel);*/
        console.log(sendViewModel);
        $.post(actionUrl, sendViewModel).done(function (data) {
            
            if (data === true) {
                    placeholderElement.find('.modal').modal('hide');
                location.reload();
                return;
            }
            var newBody = $('.modal-body', data);
            //var isValid = $('#IsValid').val();
            //var something = isValid == 'True';
            /*var isValid = newBody.find('[name="IsValid"]').val() == "true";*/
            placeholderElement.find('.modal-body').replaceWith(newBody);
            
            /*var isValid = ('#IsValid').val();*/
            /*location.reload();*/
            
            
            
            //    $.ajax({
            //            type: 'POST',
            //            url: actionUrl,
            //            dataType: 'json',
            //           /* contentType: 'application/json',*/
            //            /* contentType: 'application/x-www-form-urlencoded; charset=UTF-8',*/

            //            /* data: JSON.stringify(sendViewModel, Object()),*/
            //            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',

            /* stack overflow addition solution vv
                     //contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                     //    data: sendViewModel,*/
            //            
            /* data: JSON.stringify(sendViewModel),*/
            //          /*  data: sendViewModel, */
            //            /*data: sendViewModel,*/
            //            success: function (result) {
            //                console.log('Data received: ');
            //                console.log(result);
            /*}*/


        });
    });
});
        
    

        

        