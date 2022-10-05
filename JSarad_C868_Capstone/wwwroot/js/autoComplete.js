//$((function (e) {
//    var target = e.target;
//    var controller = $(target).data('controller');
//    var action = $(target).data('action');;
//    var url = "/" + controller + "/" + action + "/";
    
//    $("#txtToComplete").autocomplete ({
//        source: function (request, response) {
//            $.ajax({
//                url: url,
//                type: "POST",
//                dataType: "json",
//                data: { "prefix": request.term },
//                success: function (data) {
//                    response($.map(data, function (item) {
//                        return {
//                            label: item.autoComplete, value: item.txtToComplete
//                        };
//                    }))
//                }
//            })
//        },
//        messages: {
//            noResults: "", results: ""
//        }
    //            error: function (response) {
    //                alert(response.responseText);
    //            },
    //            failure: function (response) {
    //                alert(response.responseText);
    //            }
    //        });
    //    },
    //    select: function (e, i) {
    //        $("#hfCustomer").val(i.item.val);
    //    },
    //    minLength: 0
    //}).focus(function () {
    //    $(this).autocomplete("search");
    //});
        //on('input', (e) => {
        //source: function (request, response) {

        //}
        //controller = $(target).data('controller');
        //action = $(target).data('action');
        //url = "/" + controller + "/" + action + "/"
        //$.ajax({
        //    type: "POST",
        //    url: url,
        //    success: function (data) {
        //    }

        //});
    });
        
}()));