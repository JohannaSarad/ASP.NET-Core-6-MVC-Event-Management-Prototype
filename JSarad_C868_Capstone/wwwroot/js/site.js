
//autoComplete user input text
function AutoComplete() {
    
    var controller = document.getElementById("completeTxt").getAttribute("controller");
    var action = document.getElementById("completeTxt").getAttribute("action");
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
            console.log(i.item.val);
            console.log(i.item.id);
            $("#autoId").val(i.item.id);
        },
        minLength: 0
    }).focus(function () {
        $(this).autocomplete("search");
    });
};


//Select and highlight table rows (response = name or type, returns id)
$((function () {
    $(".selectable").on('click', (e) => {
        e.preventDefault
        var selectedRow = e.target.parentElement;
        var id = $(selectedRow).data('id');
        var index = $(selectedRow).data('index');
        var controller = selectedRow.getAttribute("controller");
        var action = selectedRow.getAttribute("action");
        var url = "/" + controller + "/" + action + "/" + id;
        
        var trArray = selectedRow.parentElement.getElementsByTagName('tr');
        for (var row = 0; row < trArray.length; row++) {
            if (row == index)
            {
                tdArray = selectedRow.getElementsByTagName('td');
                for (var cell = 0; cell < tdArray.length; cell++)
                {
                    tdArray[cell].style.backgroundColor = "yellow";
                    //FIX ME!!! need to remove hover for these cells
                }
            }
            else
            {
                tdArray = trArray[row].getElementsByTagName('td');
                for (var cell = 0; cell < tdArray.length; cell++)
                {
                    tdArray[cell].style.backgroundColor = "#f8f9fa";
                    //FIX ME!!! need to add hover back into these cells
                   
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
        
/*Modify Object Function (replaces index placeholder with popup modal, checks if object is new or update,
  calls for serverside validation on save, runs loop to repopulate modal on serverside validation fail, 
  alerts if object is not selected, closes modal and reloads list on serverside validation success)*/
$(function () {
    var placeholderElement = $('#PlaceHolderHere');
    var id;
    var controller;
    var action;

    $('button[data-bs-toggle="ajax-modal"]').click(function (e) {
        target = e.target;
        controller = $(target).data('controller');
        action = $(target).data('action');
        var addOrEdit = $(target).data('modify-type');
        
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
        
        var url = "/" + controller + "/" + action + "/" + id;
        console.log(url);
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        })
    })

    placeholderElement.on('click', '[data-bs-save="modal"]', function (event) {
        event.preventDefault();
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var sendViewModel = form.serialize();
       
        $.post(actionUrl, sendViewModel).done(function (data) {
            if (data === true) {
                placeholderElement.find('.modal').modal('hide');
                location.reload();
                return;
            }
            var newBody = $('.modal-body', data);
            placeholderElement.find('.modal-body').replaceWith(newBody);
        });
    });
});

$(function () {
    $("#eventSchedule").on('click', (e) => {
        e.preventDefault
        target = e.target;
        var id = $('#selectedId').val();
        controller = $(target).data('controller');
        action = $(target).data('action');
        

        if (id == 0 || id == null || id == "") {
            alert("Please Select a Record to View");
            return;
        }
        var url = "/" + controller + "/" + action + "/" + id;
        window.location.replace(url);
    });
}());

$(function () {
    $("#scheduleEmployee").on('click', (e) => {
        e.preventDefault
        target = e.target;
        var id = $('#selectedId').val();
        controller = $(target).data('controller');
        action = $(target).data('action');


        if (id == 0 || id == null || id == "") {
            alert("Please Select a Record to View");
            return;
        }
        var url = "/" + controller + "/" + action + "/" + id;
        window.location.replace(url);
        //add date validation to controller and respond here with a modal or an alert
    });
}());


        