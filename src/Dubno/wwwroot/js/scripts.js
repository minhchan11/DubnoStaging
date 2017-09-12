
$(document).ready(function () {
    //this will call the approve post method which will update the approval and pending booleans to allow display on the index page.
    $('.post').click(function () {
        var route = '#post-' + this.value;
        console.log(this.value);
        $.ajax({
            type: 'POST',
            url: '/Home/ApprovePost/' + this.value,
            success: function (result) {
                $(route).html(result);
            }
        });
    });
});


//navbar fixed position, changes the class in order to get the scroll affect

$(document).ready(function () {
    $(".navbar").hide();

    if (window.location.pathname == '/Home/Index') {

        $(window).scroll(function () {
            if ($(this).scrollTop() > 600) {
                $('.navbar').fadeIn();
                $(".navbar-me").addClass("fixed-me");
            }
            else {
                $('.navbar').fadeOut();
            }
        });
    }
    else {
        $('.navbar').fadeIn();
        $(".navbar-me").addClass("fixed-me");
    }
});

