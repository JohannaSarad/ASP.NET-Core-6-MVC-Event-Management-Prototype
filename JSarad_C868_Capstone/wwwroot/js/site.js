//function LogIn()
//{
//    var placeholderElement = $('#PlaceHolderHere');
//}



//autoComplete user input text for client name
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
                    tdArray[cell].style.backgroundColor = "#71EEDD";
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
        
//Modify Object Function

/*replaces index placeholder with popup modal, checks if object is new or update,
  calls for serverside validation on save, runs loop to repopulate modal on serverside validation fail, 
  alerts if object is not selected, closes modal and reloads list on serverside validation success)*/
$(function () {
    var placeholderElement = $('#PlaceHolderHere');
    console.log(placeholderElement);
    var id;
    var controller;
    var action;

    //onclick from index to open modal
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

    //onclick to save modal form information
    placeholderElement.on('click', '[data-bs-save="modal"]', function (event) {
        
        event.preventDefault();
        /*sections formatting datetimes are only used for  _ModifyEventModalPartial*/

        //store user selected date without time and times without date from datepicker (for _ModifyEventModalPartial)
        //var returnDate = $('#datepicker').val();
        //var returnStart = $('#startTime').val();
        //var returnEnd = $('#endTime').val()

        //append date to times store as values for hidden fields so they will pass validation on send (for _ModifyEventModalPartial)
        //$('#date').val($('#datepicker').val() + " " + $('#startTime').val());
        //$('#start').val($('#datepicker').val() + " " + $('#startTime').val());
        //$('#end').val($('#datepicker').val() + " " + $('#endTime').val());

        //remove fields passing only time and only date so form will pass validation (for _ModifyEventModalPartial)
        //$('#datepicker').remove();
        //$('#startTime').remove();
        //$('#endTime').remove();

        //get form and form action and serialize (for Employee, Client, and Event _Modify""ModalPartials)
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var sendViewModel = form.serialize();

        //recreate date and time fields with only date and times (returns to new body if validation fails for _ModifyModalPartial)
        /*$('#datepicker').val('#date');*/
        ////$('#startTime').val(returnStart);
        ////$('#endTime').val(returnEnd);
        alert(sendViewModel);
        
        //alert($('#datepicker').val() + " " + $('#endTime').val());
       /* alert(form.Event.EventDate.Date + " " + form.Event.StartTime.TimeOfDay);*/
        //alert(form.Event.EventStart.Date);
        
        /*alert(sendViewModel);*/

        $.post(actionUrl, sendViewModel).done(function (data) {
            if (data === true) {
                placeholderElement.find('.modal').modal('hide');
                location.reload();
                return;
            }

            var newBody = $('.modal-body', data);
            alert(newBody);
            
            placeholderElement.find('.modal-body').replaceWith(newBody);
        });
    });
});

//gets id from view and posts to open details&scheduling for events and employees on button click
$(function () {
    $("#details").on('click', (e) => {
        e.preventDefault
        target = e.target;
        var id = $('#selectedId').val();
        controller = $(target).data('controller');
        action = $(target).data('action');
        var form = $(this).parents('.modal').find('form');

        if (id == 0 || id == null || id == "") {
            alert("Please Select a Record to View");
            return;
        }
        var url = "/" + controller + "/" + action + "/" + id;
       /* window.location.replace(url);*/
        /*location.assign(url);*/
        window.location.href = url;
        
    });
}());

//checks input for AddSchedule
//$(function () {
//    $("#addSchedule").on('click', (e) => {
//        e.preventDefault
//        target = e.target;
//        /*var id = $('#selectedId').val();*/
//        console.log(id);
//        /*var start = $('#start').val();*/
//        console.log(start);
//        var end = $('#end').val();
//        console.log(end);
//        controller = $(target).data('controller');
//        action = $(target).data('action');
//        var obj = {};
//        obj.id = $('#selectedId').val();
//        obj.start = $('#start').val();
//        obj.end = $('#end').val();

//        if (id == 0 || id == null || id == "") {
//            alert("Please Select an Employee for this Schedule");
//            return;
//        }
//        var url = "/" + controller + "/" + action + "/" + id;
//        //$.post(url, Json.Stringify(obj)).done(function (data) {
//        //    if (data === true) {
//        //        return
//        //    }
//        //});

//        $.ajax({
//            type: 'POST',
//            url: url
//            /*data: { "employeeId": id, "start": start, "end": end}*/
//            //dataType: 'json',
//            //success: AjaxSucceeded,
//            //error: AjaxFailed
//        });
//    })
//})

//$(function () {
//    $("#hardSetValues").on('click')
//}
        