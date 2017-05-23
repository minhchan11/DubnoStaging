
    $(document).ready(function () {
        //get click function- update is done in controller
        $('.post').click(function () {
            var route = '#post-' + this.value;
            $.ajax({
                type: 'GET',
                url: '/Home/ApprovePost/' + this.value,
                success: function (result) {
                $(route).html(result);
                }
            });
        });


        //$(".delete-post").click(function () {
        //    $.ajax({
        //        type: "POST",
        //        url: 'Home/Delete/' + this.value,
        //        success: function (result) {
        //            console.log("result" + result);
        //            var postId = result.id.toString();
        //            console.log("postId" + postId);
        //            $('.each-' + postId).remove();
        //        }
        //    });
        //});
    });
