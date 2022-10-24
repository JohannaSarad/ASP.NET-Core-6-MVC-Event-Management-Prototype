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
                $(".selectedId").val(id);
            }
        })
            

    });
}()));
                
        
        
    

        

        