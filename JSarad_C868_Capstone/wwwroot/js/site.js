
//autoComplete user input text for client name
function AutoComplete() {

    //FIX ME!! autocomplete not working until the second letter
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
                $("#selectedIndex").val(index);
            }
        })
    });
}()));
        
//Modify Object Function

/* For _Modify""ModalPartials (Client, Employee, Event) replaces index placeholder div in In Index.cshtml for Client, 
 * Employee and Event with popup modal, 
 */
$(function () {
    var placeholderElement = $('#PlaceHolderHere');
    console.log(placeholderElement);
    
    var id;
    var controller;
    var action;

    //onclick from index to open modal recieves controller action and modify type from caller
    $('button[data-bs-toggle="ajax-modal"]').click(function (e) {
        target = e.target;
        controller = $(target).data('controller');
        action = $(target).data('action');
        var addOrEdit = $(target).data('modify-type');

        //check if object is new or being updated
        if (addOrEdit == 'Edit')
        {
            id = $("#selectedId").val();
            //alerts if object is not selected
            if (id == null || id == "") {
                    var errorDiv = document.getElementById("errorModalBody");
                    errorDiv.innerHTML = "Please Select a Record to Edit";
                    $('#errorModal').modal('show');
                return;
            }
        }
        else
        {
            id = 0;
        }
        
        var url = "/" + controller + "/" + action + "/" + id;
        
        //displays modal
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    })

    //onclick to save modal form information
    placeholderElement.on('click', '[data-bs-save="modal"]', function (event) {
        event.preventDefault();
        
        //get form and form action and serialize (for Employee, Client, and Event _Modify""ModalPartials)
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var sendViewModel = form.serialize();

        //call to serverside validation on save
        $.post(actionUrl, sendViewModel).done(function (data) {
            if (data === true) {
                placeholderElement.find('.modal').modal('hide');
                location.reload();
                return;
            }

            //serverside validation fail reload modal body with validation errors
            var newBody = $('.modal-body', data);
            placeholderElement.find('.modal-body').replaceWith(newBody);
        });
    });
});

//Open Add Schedule
//gets id from view and posts to open details&scheduling for /Event/AddSchedule/{id} and /Employee/EmployeeSchedule/{id} on button click
$(function () {
    $("#details").on('click', (e) => {
        e.preventDefault
        target = e.target;
        var id = $('#selectedId').val();
        controller = $(target).data('controller');
        action = $(target).data('action');

        //alerts if no id is selected
        if (id == 0 || id == null || id == "") {
            /*alert("Please Select a Record to View");*/
            var errorDiv = document.getElementById("errorModalBody");
            errorDiv.innerHTML = "Please Select a Record to View";
            $('#errorModal').modal('show');
            return;
        }
        var url = "/" + controller + "/" + action + "/" + id;
        window.location.replace(url);
    });
}());

//function for delete action without selectable rows called from (Event.AddSchedule)
$((function () {

    var target;
    var pathToDelete;
    var id;
    var controller;
    var action;
    var index

    $('body').append(`
                    <div class="modal fade" id="deleteUnselectableModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-bs-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                    </button>
                                    <h4 class="modal-title" id="myModalLabel">Warning</h4>
                                </div>
                                <div class="modal-body delete-modal-body">
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-default" data-bs-dismiss="modal" id="cancel-delete-unselectable">Cancel</button>
                                    <button type="submit" class="btn btn-danger" id="confirm-delete-unselectable">Delete</button>
                                </div>
                            </div>
                        </div>
                    </div>`);

    /* gets action, controller, id, index, and body mesage values from sender in Event.AddSchedule.cshtml and displays 
     * delete modal
     */
    $(".delete-unselectable").on('click', (e) => {
        e.preventDefault();
        target = e.target;
        id = $(target).data('id');
        index = $(target).data('index');
        controller = $(target).data('controller');
        action = $(target).data('action');
        var bodyMessage = $(target).data('body-message');
        pathToDelete = "/" + controller + "/" + action + "/" + id;

        $(".delete-modal-body").text(bodyMessage);
        $("#deleteUnselectableModal").modal('show');
    });

    /* posts to Event.RemoveSchecule to remove employee schedule from AddSchedule on modal confirm delete click, closes modal
     * and
     * removes selected row from view 
     */
    $("#confirm-delete-unselectable").on('click', () => {
        $.ajax({
            type: "POST",
            url: pathToDelete,
            success: function () {
                $("#deleteUnselectableModal").modal("hide");
                $("#row_" + id).remove();
            }
        });
    });
}()));
               