//function for delete action with selectable rows called from Index.cshtml (Client, Employee, Event)
$((function () {
    
    var target;
    var pathToDelete;
    var id;
    var controller;
    var action;

    $('body').append(`
                    <div class="modal fade" id="deleteModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
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
                                    <button type="button" class="btn btn-default" data-bs-dismiss="modal" id="cancel-delete">Cancel</button>
                                    <button type="submit" class="btn btn-danger" id="confirm-delete">Delete</button>
                                </div>
                            </div>
                        </div>
                    </div>`);

    /*gets action, controller, id, index, and body mesage values from senders and displays
     * delete modal
     */
    $(".delete").on('click', (e) => {
        e.preventDefault();

        target = e.target;
        //id = $(target).data('id');
        id = $("#selectedId").val();
        index = $("#selectedIndex");
        controller = $(target).data('controller');
        action = $(target).data('action');
        var bodyMessage = $(target).data('body-message');
        pathToDelete = "/" + controller + "/" + action + "/" + id;
        $(".delete-modal-body").text(bodyMessage);
        $("#deleteModal").modal('show');
        
    });

    /* posts to DeletePOST action respectively for Client, Employee, and Event on modal confirm delete click 
     * based on id selected from view, closes modal, and removes selected row from view
     * */
    $("#confirm-delete").on('click', () => {
        $.ajax({
            type: "POST",
            url: pathToDelete,
            success: function () {
                $("#deleteModal").modal("hide");
                $("#row_" + id).remove();
                $("#selectedId").val("");
            }
        });
    });
}()));