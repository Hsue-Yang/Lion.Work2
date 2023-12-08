var erpNews = {
    rotateSeconds: 4000,    // [輪播秒數]
    newsTimer: 0,           // 
    newsCNTotal: 1,         // [留言總數]
    objDiv: 1,              // [id名稱為divContentWrapper的DIV jquery物件]
    newsHeight: 1,          // [每筆留言的高度] ※提供點擊向上或向下按鈕的高度計算
    newsMax: 0,             // [最後一筆留言的高度] ※以[留言總數]及[每筆留言的高度]計算出
    newsCurrent: 0,         // [目前留言位置的高度] ※當高度等於0表示為第一筆留言
    init: function () {
        this.newsCNTotal = $("#divContent ul li").size();
        this.objDiv = $("#divContentWrapper");

        if (this.objDiv[0]) {
            this.newsHeight = parseInt($("#divContentWrapper ul li").css("height").replace("px", "")) + 1;
        }
        this.newsMax = ((this.newsCNTotal - 1) * this.newsHeight) * -1;
        this.newsCurrent = 0;

        $("#divToolbar #imgShow").bind("click", function (B) {
            var A = $(this).attr("class");
            if (A === "news-show on") {
                if ($(".divContentWrapper-on").length > 0) {
                    $(this).removeClass("on");
                    $("#divContentWrapper").removeClass("divContentWrapper-on");
                }
            } else {
                $(this).addClass("on");
                $("#divContentWrapper").addClass("divContentWrapper-on");
            }
            B.preventDefault();
        });

        $(".news-down", $("#divToolbar")[0]).live("click", function () {
            if (erpNews.newsCurrent !== erpNews.newsMax) {
                erpNews.newsCurrent = erpNews.newsCurrent - erpNews.newsHeight;
                $("#divContent ul").animate({ top: erpNews.newsCurrent }, 400);
            } else {
                erpNews.newsCurrent = 0;
                $("#divContent ul").animate({ top: erpNews.newsCurrent }, 1000);
            }
        });

        $(".news-up", $("#divToolbar")[0]).live("click", function () {
            if (erpNews.newsCurrent !== 0) {
                erpNews.newsCurrent = erpNews.newsCurrent + erpNews.newsHeight;
                $("#divContent ul").animate({ top: erpNews.newsCurrent }, 400);
            }
        });

        newsTimer = window.setInterval("erpNews.startNewsRotation();", this.rotateSeconds);
    },
    startNewsRotation: function () {
        if ($(".divContentWrapper-on").length == 0) {
            if (this.newsCurrent !== this.newsMax) {
                this.newsCurrent = this.newsCurrent - this.newsHeight;
                $("#divContent ul").animate({ top: this.newsCurrent }, 400);
            } else {
                this.newsCurrent = 0;
                $("#divContent ul").animate({ top: this.newsCurrent }, 1000);
            }
        }
    }
};

(function (A) {
    A.fn.ellipsis=function() {
        return this.each(function() {
            var F=A(this);
            if(F.css("overflow")=="hidden") {
                var I=F.html();
                var C=true;
                var D=A(this.cloneNode(true)).hide().css("position","absolute").css("overflow","visible").width(C?F.width():"auto").height(C?"auto":F.height());
                F.after(D);
                function B() {
                    return D.height()>F.height();
                }
                function E() {
                    return D.width()>F.width();
                }
                var G=C?B:E;
                while(I.length>0&&G()) {
                    var H=I.split(" ");
                    H.pop();
                    I=H.join(" ");
                    D.html(I+" &#155;");
                }
                F.html(D.html());
                D.remove();
            }
        })
    }
})(jQuery);

$(document).ready(function () {
    $("#divContentWrapper a").ellipsis();
    erpNews.init();
});