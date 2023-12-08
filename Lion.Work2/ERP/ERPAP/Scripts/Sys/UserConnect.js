var _formElement;

function UserConnectForm_onLoad(formElement) {
    _formElement = formElement;
    var table = $('#UserConnectTable', _formElement);

    if (table.length > 0) {
        table.hide();
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('.tblsearch:eq(1),table[id=Pager]').each(function () { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 0 });
    }

    $('.BaseContainer').css('z-index', 0);

    $('#TimeBegin, #TimeEnd', $('table.tblsearch', _formElement)).combobox({
        isRemoveButton: false
    });

    return true;
}

function PageSize_onEnter(srcElement) {
    SelectButton_onClick();
    return true;
}

function SelectButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (result) {
        _formElement.submit();
    }
}