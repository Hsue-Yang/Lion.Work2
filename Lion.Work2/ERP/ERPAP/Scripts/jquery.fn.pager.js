;
jQuery.fn.extend({
    pager: function(param) {
        var self = this;
        var topPageIndex = $('select[name=PageIndex]', self[0]);
        var bottomPageIndex = $('select[name=PageIndex]', self[1]).removeAttr('name');
        var topPageSize = $('input[name=PageSize]', self[0]);
        var bottomPageSize = $('input[name=PageSize]', self[1]).removeAttr('name');

        var pagerIndexChange = function (srcElement) {
            eval('if(typeof(SetValidate)===\'function\'){ SetValidate(); }');
            var result = _FormValidation();
            $('#ExecAction').val(_ActionTypeSelect);
            if (result) {
                eval('if(typeof(PageIndex_onChange)===\'function\'){ result = PageIndex_onChange(srcElement); }');
            }
            if (result) {
                var formElement = _GetParentElementByTag(srcElement, 'form');
                formElement.submit();
            }
            return true;
        };

        var pagerChange = function() {
            eval('if(typeof(SetValidate)===\'function\'){ SetValidate(); }');
            var result = _FormValidation();
            if (result) {
                var name = $(this).attr('name');
                var pageIndex = topPageIndex.val();
                var last = $('option', topPageIndex).length;

                pageIndex = parseInt(pageIndex * 1, 10);

                switch (name) {
                    case 'FirstPagerButton':
                    {
                        pageIndex = 1;
                        break;
                    }
                    case 'PrevPagerButton':
                    {
                        pageIndex = pageIndex - 1;
                        break;
                    }
                    case 'NextPagerButton':
                    {
                        pageIndex = pageIndex + 1;
                        break;
                    }
                    case 'LastPagerButton':
                    {
                        pageIndex = last;
                        break;
                    }
                }

                if (pageIndex <= 0) {
                    pageIndex = 1;
                } else {
                    if (pageIndex > last) {
                        pageIndex = last;
                    }
                }

                topPageIndex.val(pageIndex);
                bottomPageIndex.val(pageIndex);
                var formElement = _GetParentElementByTag(this, 'form');
                formElement.submit();
            }
            return false;
        };

        topPageIndex.change(function() {
            bottomPageIndex.val(this.value);
            pagerIndexChange(this);
        });
        bottomPageIndex.change(function() {
            topPageIndex.val(this.value);
            pagerIndexChange(this);
        });

        topPageSize.blur(function () {
            bottomPageSize.val(this.value);
            _TextBox_onBlur(this);
        });
        bottomPageSize.blur(function () {
            topPageSize.val(this.value);
            _TextBox_onBlur(this);
        });
        topPageSize.keypress(function () {
            bottomPageSize.val(this.value);
            _TextBox_onKeyPress(this);
        });
        bottomPageSize.keypress(function () {
            topPageSize.val(this.value);
            _TextBox_onKeyPress(this);
        });

        $('input[name=FirstPagerButton]', self).val(_Symbol.First).click(pagerChange);
        $('input[name=PrevPagerButton]', self).val(_Symbol.Prev).click(pagerChange);
        $('input[name=NextPagerButton]', self).val(_Symbol.Next).click(pagerChange);
        $('input[name=LastPagerButton]', self).val(_Symbol.Last).click(pagerChange);
        $('span[name=StrPager1]', self).html(_PagerMessage.StrPager1.format(param.TotalCount));
        $('span[name=StrPager2]', self).html(_PagerMessage.StrPager2.format(param.PageCount));
        $('span[name=StrPager3]', self).html(_PagerMessage.StrPager3);
        $('input[id=PageSize]', self).attr('titlename', _PageSizeTitle);
        $('input[id=PageSize]', self).each(function () {
            $(this).attr('title', _getTextBoxTitle(this));
        });
    }
});