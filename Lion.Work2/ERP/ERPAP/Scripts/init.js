$(function () {	
    //datepicker
    $('.dp').datepicker({
        language: 'zh-TW',
        autoclose: true
    });
	
    $('#topnav ul>li:eq(6) span').html('其他作業平台');

    $('#topnav ul>li:eq(8)').children('a').children('span').replaceWith('<span>旅天下網站</span>');
	
    $('#topnav ul>li:eq(9) ul>li:eq(1)>a').attr('href', 'https://erp.liontravel.com/mail/index.htm');

    $('#topnav ul>li:eq(10),#topnav ul>li:eq(11)').remove();

	var head  = document.getElementsByTagName('head')[0];
    var link  = document.createElement('link');
    link.rel  = 'shortcut icon';

    if (document.domain.indexOf('liontravel.com.tw') > -1) {
        $('#sitename>a').attr('href', 'http://userp.liontravel.com.tw/Pub/Bulletin');
        $('#topnav ul>li:eq(0)>div>ul').append('<li><a href="http://userp.liontravel.com.tw/Pub/Bulletin?val2=ko_KR" target="_self">한국어</a></li>');
        $('#topnav ul>li:eq(1)>a').attr('href', 'http://utourerp.liontravel.com.tw/Bulletin/SearchList');
        $('#topnav ul>li:eq(2)>a').attr('href', 'http://uvisaerp.liontravel.com.tw/SEND/Application');
        $('#topnav ul>li:eq(3)>a').attr('href', 'http://uam.liontravel.com.tw/PunchClock/Internal');
        $('#topnav ul>li:eq(7)>div.subnav a:eq(1)').attr('href', 'https://ub2b.liontravel.com/login_otp.aspx');
        $('#topnav ul>li:eq(8)>div.subnav a:eq(0)').attr('href', 'https://uwww.gounitravel.com/');
        $('#topnav ul>li:eq(8)>div.subnav a:eq(1)').attr('href', 'https://ub2b.gounitravel.com/login_otp.aspx');
        $('#topnav>ul').append(
            '<li class="hasmenu">' +
            '<a href="javascript:void(0);" class="navtxt"><span>商品部網站</span></a>' +
            '<div class="subnav"><ul>' +
            '<li><a target="_blank" href="https://lionselect.shoplineapp.com">商品部網站</a></li>' +
            '<li><a target="_blank" href="https://docs.google.com/forms/d/e/1FAIpQLSfwA78hOM7zPc_LL_D_qEvwxwsQiQPKWrHlovkY-wV-M-ySjw/viewform">推薦碼及後台查閱申請</a></li>' +
            '<li><a target="_blank" href="https://docs.google.com/presentation/d/1JX_2gDOCg0ew0OBzd7Dtu8qyEfWE1UNEkgOCVY2KPWI/edit?usp=sharing">企業戶申請</a></li>' +
            '<li><a target="_blank" href="https://docs.google.com/presentation/d/1Zpy9NfqaW4R_ICHlt4bG1r8JSZ8P_cW4YN_c0Il7I3A/edit?usp=sharing">禮贈品採購申請</a></li>' +
            '</ul></div></li>');
        $('#topnav>ul').append('<li><a target="_blank" href="https://uwelfare.liontravel.com/zh-tw/activity">福委</a></li>');
        $('#topnav>ul').append(
            '<li class="hasmenu">' +
            '<a href="javascript:void(0);" class="navtxt"><span>雄獅保經專區</span></a>' +
            '<div class="subnav"><ul>' +
            '<li><a target="_blank" href="https://drive.google.com/drive/folders/10DQ7W5nZ06nbosstdJ-Nq-IyLlSsXVa9?usp=drive_link">保險資料文件</a></li>' +
            '</ul></div></li>');
        $('ul.dropdown-menu li a[href$=Logout]').attr('href', 'http://userp-auth.liontravel.com.tw/connect/logout?post_logout_redirect_uri=http://userp.liontravel.com.tw/Home/Logout');
        
        link.href = 'http://userp.liontravel.com.tw/favIcon.ico';
        
    } else {
        $('#sitename>a').attr('href', 'https://serp.liontravel.com/Pub/Bulletin');

        if (document.domain.indexOf('127.0.0.1') > -1) {
            $('#topnav ul>li:eq(0)>div>ul').append('<li><a href="http://127.0.0.1:8888/Pub/Bulletin?val2=ko_KR" target="_self">한국어</a></li>');
        }
        else {
            $('#topnav ul>li:eq(0)>div>ul').append('<li><a href="https://serp.liontravel.com/Pub/Bulletin?val2=ko_KR" target="_self">한국어</a></li>');
        }

        $('#topnav ul>li:eq(1)>a').attr('href', 'https://tourerp.liontravel.com/Bulletin/SearchList');
        $('#topnav ul>li:eq(2)>a').attr('href', 'https://visaerp.liontravel.com/SEND/Application');
        $('#topnav ul>li:eq(3)>a').attr('href', 'https://am.liontravel.com/PunchClock/Internal');
        $('#topnav ul>li:eq(7)>div.subnav a:eq(1)').attr('href', 'https://b2b.liontravel.com/login_otp.aspx');
        $('#topnav ul>li:eq(8)>div.subnav a:eq(0)').attr('href', 'https://www.gounitravel.com/');
        $('#topnav ul>li:eq(8)>div.subnav a:eq(1)').attr('href', 'https://b2b.gounitravel.com/login_otp.aspx');
        $('#topnav>ul').append(
            '<li class="hasmenu">' +
            '<a href="javascript:void(0);" class="navtxt"><span>商品部網站</span></a>' +
            '<div class="subnav"><ul><li><a target="_blank" href="https://lionselect.shoplineapp.com">商品部網站</a></li>' +
            '<li><a target="_blank" href="https://docs.google.com/forms/d/e/1FAIpQLSfwA78hOM7zPc_LL_D_qEvwxwsQiQPKWrHlovkY-wV-M-ySjw/viewform">推薦碼及後台查閱申請</a></li>' +
            '<li><a target="_blank" href="https://docs.google.com/presentation/d/1JX_2gDOCg0ew0OBzd7Dtu8qyEfWE1UNEkgOCVY2KPWI/edit?usp=sharing">企業戶申請</a></li>' +
            '<li><a target="_blank" href="https://docs.google.com/presentation/d/1Zpy9NfqaW4R_ICHlt4bG1r8JSZ8P_cW4YN_c0Il7I3A/edit?usp=sharing">禮贈品採購申請</a></li>' +
            '</ul></div></li>');
        $('#topnav>ul').append('<li><a target="_blank" href="https://welfare.liontravel.com/zh-tw/index">福委</a></li>');
        $('#topnav>ul').append(
            '<li class="hasmenu">' +
            '<a href="javascript:void(0);" class="navtxt"><span>雄獅保經專區</span></a>' +
            '<div class="subnav"><ul>' +
            '<li><a target="_blank" href="https://drive.google.com/drive/folders/10DQ7W5nZ06nbosstdJ-Nq-IyLlSsXVa9?usp=drive_link">保險資料文件</a></li>' +
            '</ul></div></li>');
        $('ul.dropdown-menu li a[href$=Logout]').attr('href', 'https://serp-auth.liontravel.com/connect/logout?post_logout_redirect_uri=https://serp.liontravel.com/Home/Logout');

        link.href = 'https://serp.liontravel.com/favIcon.ico';
    }

    head.appendChild(link);
	
    var selection1 = $('#function-bar .dropdown-menu');
    var selection2 = $('#topnav > ul');
    var selection2Sub = $('#topnav .hasmenu .navtxt');
    var selection3 = $('#nav > ul');
    var selection3Sub = $('#nav .hasmenu .navtxt');

    //#region menu 位置重新計算
    if (((navigator.userAgent.match(/iPhone/i)) || (navigator.userAgent.match(/iPod/i))) == null) {
        $('#nav > ul > li.hasmenu').hover(function () {
            var left = 0,
                hasmenuAbsoluteLeft = $(this).offset().left,
                winWidth = $(window).width(),
                subnav = $('div.subnav', this),
                subnavWidth = 0;
            
            $('div.subnav ul', this).each(function (idx, el) {
                if ($('li', el).length > 0) {
                    subnavWidth += ($(el).width() + 22);
                }
            });
            
            if ((hasmenuAbsoluteLeft + subnavWidth) > winWidth) {
                left = winWidth - hasmenuAbsoluteLeft - subnavWidth - 22;
            }

            subnav
                .attr('resize', 'true')
                .css({
                    left: left + 'px',
                    width: subnavWidth
                });
        });
    }
    //#endregion

    //M版右上角選單
    $('#navicon1').click(
        function () {
            selection2.hide();
            selection3.hide();
            $('#navicon2').removeClass('active');
            $('#navicon3').removeClass('active');
            if (selection1.is(':visible')) {
                selection1.hide();
                $(this).removeClass('active');
            } else {
                selection1.show();
                $(this).addClass('active');
            }
        }
    );

    $('#navicon2').click(
        function () {
            selection1.hide();
            selection3.hide();
            $('#navicon1').removeClass('active');
            $('#navicon3').removeClass('active');
            if (selection2.is(':visible')) {
                selection2.hide();
                $(this).removeClass('active');
            } else {
                selection2.show();
                $(this).addClass('active');
            }
        }
    );

    var selection2Subnav = $('#topnav .hasmenu .subnav');
    selection2Sub.click(
        function () {
            var subMenu = $(this).siblings($('.subnav'));
            selection2Subnav.not(subMenu).hide();
            if (subMenu.is(':visible')) {
                subMenu.hide();
                $(this).removeClass('active');
            } else {
                subMenu.show();
                $(this).addClass('active');
            }
        }
    );

    $('#navicon3').click(
        function () {
            selection1.hide();
            selection2.hide();
            $('#navicon1').removeClass('active');
            $('#navicon2').removeClass('active');
            if (selection3.is(':visible')) {
                selection3.hide();
                $(this).removeClass('active');
            } else {
                selection3.show();
                $(this).addClass('active');
            }
        }
    );

    var selection3Subnav = $('#nav .hasmenu .subnav');
    selection3Sub.click(
        function () {
            var subMenu = $(this).siblings($('.subnav'));
            selection3Subnav.not(subMenu).hide();
            if (subMenu.is(':visible')) {
                subMenu.hide();
                $(this).removeClass('active');
            } else {
                var left = 0,
                    hasmenuAbsoluteLeft = $(this).closest('li.hasmenu').offset().left,
                    winWidth = $(window).width(),
                    subnavWidth = 0;
                
                subMenu.show();

                $('ul', subMenu).each(function (idx, el) {
                    if ($('li', el).length > 0) {
                        subnavWidth += ($(el).width() + 22);
                    }
                });

                if ((hasmenuAbsoluteLeft + subnavWidth) > winWidth) {
                    left = winWidth - hasmenuAbsoluteLeft - subnavWidth - 22;
                }

                subMenu
                    .attr('resize', 'true')
                    .css({
                        left: left + 'px',
                        width: subnavWidth
                    });

                $(this).addClass('active');
            }
        }
    );

    // for android chrome browser
    if (navigator.userAgent.toLowerCase().indexOf('android') > -1) {
        $('#nav ul li.hasmenu').hover(function(e) {
            var subnav = $('.subnav', this);
            var subMenu = subnav.siblings($('.subnav'));
            selection3Subnav.not(subMenu).hide();
            subnav.show();
        }, function(e) {
            var subnav = $('.subnav', this);
            subnav.hide(500);
        });

        $('#nav .hasmenu .subnav > ul > li a').bind('touchstart', function(e) {
            window.location.href = $(this).attr('href');
        });
    } else {
        var style = document.createElement('style');
        style.type = 'text/css';
        style.innerHTML = '#nav ul li.hasmenu:hover .subnav { display: block; }';
        document.getElementsByTagName('head')[0].appendChild(style);
    }

    function winResize() {
        var win = $(this);
        if (win.width() >= 640) {
            selection1.css('display', '');
            selection2.css('display', 'block');
            selection2Sub.siblings($('.subnav')).css('display', '');
            selection3.css('display', 'block');
            selection3Sub.siblings($('.subnav')).css('display', '');
        }
        if (win.width() >= 320 && win.width() < 900) {
            selection1.css('display', 'none');
            selection2.css('display', 'none');
            selection3.css('display', 'none');
            $('#navicon1').removeClass('active');
            $('#navicon2').removeClass('active');
            selection2Sub.removeClass('active');
            $('#navicon3').removeClass('active');
            selection3Sub.removeClass('active');
        }
    }

    $('#navicon4').click(
        function () {
            $('head').find('link[media="screen and (min-width:320px) and (max-width:1023px)"]').remove();
            $('head').find('meta[name="viewport"]').remove();
            $(window).unbind('resize', winResize);
            selection1.css('display', '');
            selection2.css('display', 'block');
            selection2Sub.siblings($('.subnav')).css('display', '');
            selection3.css('display', 'block');
            selection3Sub.siblings($('.subnav')).css('display', '');
        }
    );

    //window resize : reset
    //$(window).bind('resize', winResize);

});

//color change
function changeColor($color) {
    switch ($color) {
        case 'green':
            $('body').removeClass().addClass('greenStyle');
            break;
        case 'red':
            $('body').removeClass().addClass('redStyle');
            break;
    }
}