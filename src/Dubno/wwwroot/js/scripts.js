
    $(document).ready(function () {
        //this will call the approve post method which will update the approval and pending booleans to allow display on the index page.
        $('.post').click(function () {
            var route = '#post-' + this.value;
            console.log(this.value);
            $.ajax({
                type: 'GET',
                url: '/Home/ApprovePost/' + this.value,
                success: function (result) {
                    $(route).html(result);
                }
            });
        });

        $(".click-details").click(function () {
            $.ajax({
                type: 'GET',
                dataType: 'html',
                url: 'Home/Details/' + this.id,
                success: function (result) {
                    $('.return-details').html(result);
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
