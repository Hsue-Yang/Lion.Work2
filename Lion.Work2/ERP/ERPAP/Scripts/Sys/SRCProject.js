var _formElement;

function SRCProjectForm_onLoad(formElement) {
    _formElement = formElement;
    var table = $('#SRCProjectTable', _formElement);

    if (table.length > 0) {
        table.hide();
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('.tblsearch').each(function () { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 0 });
    }
    
    return true;
}

function TRLinkFunKey_onClick(srcElement, keys) {
    var objTR = $('tr[id="DomainGroupTable[' + keys[1] + ']"]');
    objTR.toggle();

    return false;
}

function DomainName_onChange(srcElement) {
    $('#DomainGroupID > option', _formElement).remove();
    $('#ProjectID > option', _formElement).remove();
    
    $.ajax({
        url: '/Sys/GetDomainGroupMenuList',
        type: 'POST',
        data: { domainName: $(srcElement).val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#DomainGroupID > option', _formElement).remove();
                for (var i = 0; i < result.length; i++) {
                    $('#DomainGroupID', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSRCProjectList);
            _ShowJsErrMessageBox();
        }
    });
    return true;
}

function DomainGroupID_onChange(srcElement) {
    if ($(srcElement).val() === '') {
        $('#ProjectID > option', _formElement).remove();
    }
    $.ajax({
        url: '/Sys/GetProjectID',
        type: 'POST',
        data: { domainGroupID: $(srcElement).val(), domainName: $('#DomainName').val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#ProjectID > option', $(_formElement)).remove();
                for (var i = 0; i < result.length; i++) {
                    $('#ProjectID', $(_formElement)).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSRCProjectList);
            _ShowJsErrMessageBox();
        }
    });

    return true;
}

function LinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#ProjectID').val(keys[1]);
    $('#DomainName').val(keys[2]);
    $('#DomainGroupID').val(keys[3]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function SelectButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function AddButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeAdd);
    return true;
}

function Clean_HiddenValue() {
    $('#ProjectID').val('');
    $('#DomainName').val('');
    $('#DomainGroupID').val('');
}