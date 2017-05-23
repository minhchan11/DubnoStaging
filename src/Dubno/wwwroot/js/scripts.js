
    $(document).ready(function () {
        //get click function- update is done in controller
        $('.post').click(function () {
            var path = '#post-' + this.value;
            console.log(path);
            $.ajax({
                type: 'GET',
                url: '/Home/ApprovePost/' + this.value,

                success: function (result) {
                    console.log("result" + result);
                    $(path).html(result);
                }
            });
        });
    });
