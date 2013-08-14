//remove soome.com
$(function () {
    $('center a').remove();
});

//Google+
$(function () {
    var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;
    po.src = 'https://apis.google.com/js/plusone.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);

                
});

//Facebook 
(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/en_EN/all.js#xfbml=1";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));

/*$(document).ready(
     function () {
         $("html").niceScroll({
             spacebarenabled: false, //conflict with nicEditor
             smoothscroll: true,
             scrollspeed: '90'
         });
     }
 );*/
/*  (function ($) {
      $(window).load(function () {
          $(".faqElement").mCustomScrollbar();
      });
  })(jQuery);*/

//Google analytics
var _gaq = _gaq || [];
_gaq.push(['_setAccount', 'UA-38346737-1']);
_gaq.push(['_trackPageview']);
(function () {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
})();