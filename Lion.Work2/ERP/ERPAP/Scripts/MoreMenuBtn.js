$(function () {
    var findAll = function findAll(node, selector) {
        removeId = false;
        if (node.getAttribute('id') === null) {
            node.setAttribute('id', 'ID_' + new Date().getTime());
            removeId = true;
        }
        var result = document.querySelectorAll('#' + node.getAttribute('id') + ' > ' + selector);
        if (removeId) { node.removeAttribute('id'); }
        return result;
    };
    var container = document.querySelector("#nav");
    var primary = container.querySelector("ul");
    var primaryItems = Array.apply(null, findAll(container, "ul > li:not(.-more)"));

    primary.insertAdjacentHTML(
        "beforeend",
        '\n  <li class="menu-more">\n    <button type="button" aria-haspopup="true" aria-expanded="false">\n      \u5C55\u958B <span>&#x2193;</span>\n    </button>\n  </li>\n'
    );
    var allItems = Array.apply(null, findAll(primary, "li"));
    var moreLi = primary.querySelector(".menu-more");
    var moreBtn = moreLi.querySelector("button");

    var clickHandler = function clickHandler(e) {
        e.preventDefault();
        var isExpanded = moreBtn.getAttribute("aria-expanded");

        if (isExpanded === "true") {
            doAdapt();
            moreBtn.setAttribute("aria-expanded", false);
            moreBtn.innerHTML = _JsMsg_MoreMenuOpen + " <span>&#x2193;</span>";
        } else {
            allItems.forEach(function (item) {
                item.classList.remove("js-hidden");
            });
            moreBtn.setAttribute("aria-expanded", true);
            moreBtn.innerHTML = _JsMsg_MoreMenuClose + " <span>&#x2191;</span>";
        }
    };

    moreBtn.addEventListener("click", clickHandler); // adapt tabs

    var doAdapt = function doAdapt() {
        // reveal all items for the calculation
        allItems.forEach(function (item) {
            item.classList.remove("js-hidden");
        }); // hide items that won't fit in the Primary

        var stopWidth = moreBtn.offsetWidth - 55;
        var hiddenItems = [];
        var primaryWidth = primary.offsetWidth;
        primaryItems.forEach(function (item, i) {
            if (primaryWidth >= stopWidth + item.offsetWidth) {
                stopWidth += item.offsetWidth;
            } else {
                item.classList.add("js-hidden");
                hiddenItems.push(i);
            }
        }); // toggle the visibility of More button and items in Secondary

        if (!hiddenItems.length) {
            moreLi.classList.add("js-hidden");
            container.classList.remove("--show-secondary");
            moreBtn.setAttribute("aria-expanded", false);
        }
    };

    doAdapt(); // adapt immediately on load

    window.addEventListener("resize", doAdapt); // adapt on window resize
    // hide Secondary on the outside click

    document.addEventListener("click", function (e) {
        var el = e.target;

        while (el) {
            if (el === moreBtn) {
                return;
            }

            el = el.parentNode;
        }

        doAdapt();
        moreBtn.innerHTML = _JsMsg_MoreMenuOpen + " <span>&#x2193;</span>";
        moreBtn.setAttribute("aria-expanded", false);
    });
});
