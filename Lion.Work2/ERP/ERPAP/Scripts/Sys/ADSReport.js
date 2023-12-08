var _formElement;

function ADSReportForm_onLoad(formElement) {
    _formElement = formElement;

    if ($(formElement).find('#ADSReportTable')[0] != null) {
        _TableHover('ADSReportTable', formElement);
    }

    SwitchType();

    $('#ReportType, #SysID', $('table.tblsearch', _formElement)).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: ReportType_onChange
    });

    return true;
}


function ReportType_onChange(scrElement) {
    SwitchType();
}

function SwitchType() {
    var ReportType = $('#ReportType').val();

    if (ReportType == "SysUserLoginLastTime") {
        $('#SysID').val('');
        $('#TR_Sys').hide();
    } else {
        $('#TR_Sys').show();
    }
}

function ExcelExportButton_onClick() {
    var result = _FormValidation();
    var reportType = $('#ReportType').val();
    var reportTypeName = $('#ReportType option:selected').text();
    var sysID = $('#SysID').val();

    if (result) {

        $.blockUI({ message: '<p>' + JsMsg_ProccessWaiting + '</p>'});
        $.ajax({
            type: 'POST',
            dataType: 'json',
            async: true,
            url: '/Sys/ADSReport',
            data: { sysID: sysID, reportType: reportType, reportTypeName: reportTypeName },
            success: function (response) {
                if (response.result) {
                    window.location.href = '/Sys/DownloadCsvFile';
                } else {
                    _AddJsErrMessage(response.msg);
                    _ShowJsErrMessageBox();
                }
                $.unblockUI();
            }
        });
    } else {
        return false;
    }

}