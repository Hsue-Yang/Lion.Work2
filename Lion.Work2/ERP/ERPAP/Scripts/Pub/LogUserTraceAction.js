var _formElement;

function RequiredTag() {

    var modeSelected = $('#SearchType option:selected').val();
    var userID = $('.label-user-id');
    var sysID = $('.label-sys-id');
    var controllerName = $('.label-controller-name');
    var actionName = $('.label-action-name');
    var sessionID = $('.label-session-id');
    var requestSessionID = $('.label-request-session-id');

    userID.find('i').remove();
    sysID.find('i').remove();
    controllerName.find('i').remove();
    actionName.find('i').remove();
    sessionID.find('i').remove();
    requestSessionID.find('i').remove();

    if (modeSelected === "A") {
        userID.prepend('<i>* </i>');
        sessionID.prepend('<i>* </i>');
        requestSessionID.prepend('<i>* </i>');
    }
    if (modeSelected === "B") {
        sysID.prepend('<i>* </i>');
        userID.prepend('<i>* </i>');
    }
    if (modeSelected === "C") {
        sysID.prepend('<i>* </i>');
        controllerName.prepend('<i>* </i>');
        actionName.prepend('<i>* </i>');
    }
}

function LogUserTraceActionForm_onLoad(formElement) {
    _formElement = formElement;

    var table = $('#LogUserTraceActionTable', _formElement);
    if (table.length > 0) {
        table.hide();
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('.tblsearch').each(function () { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 1 });
    }

    RequiredTag();

    $('.BaseContainer').css('z-index', 0);
}

function SearchType_onChange(srcElement) {
    RequiredTag();
}

function PageSize_onEnter(srcElement) {
    SelectButton_onClick();
    return true;
}

function SelectButton_onClick(srcElement) {
    if (_DateValidation() && _FormValidation() && _RequiredValidation()) {
        $('#ExecAction').val(_ActionTypeSelect);
        $.blockUI({ message: '' });
        _formElement.submit();
    }

    _ShowJsErrMessageBox();
    return false;
}

function Help03Button_onClick(srcElement) {
    var vMapFields = new Array(1);
    vMapFields[1] = 'UserID';
    _hISearch(vMapFields, 'newwindow', _enumPUBAP + '/Help/Ishlp03', 'Name=' + encodeURI($.trim($('#UserID').val())));
}

function _DateValidation() {
    var timeInterval = 1; //時間間隔(時)
    var startTraceDate = $('#StartTraceDate').val();
    var endTraceDate = $('#EndTraceDate').val();
    var startTraceTime = $('#StartTraceTime').val();
    var endTraceTime = $('#EndTraceTime').val();

    if ((startTraceTime.length === 5 && startTraceTime.match('^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])$')) && (endTraceTime.length === 5 && endTraceTime.match('^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])$'))) {
        startTraceDate = new Date(startTraceDate.substr(0, 4) + '-' + startTraceDate.substr(4, 2) + '-' + startTraceDate.substr(6, 2) + ' ' + startTraceTime);
        endTraceDate = new Date(endTraceDate.substr(0, 4) + '-' + endTraceDate.substr(4, 2) + '-' + endTraceDate.substr(6, 2) + ' ' + endTraceTime);
    }
    else {

        _AddJsErrMessage(JsMsg_HHmm_Error);
        _ShowJsErrMessageBox();
        return false;
    }

    if ((startTraceDate <= endTraceDate) === false) {
        _AddJsErrMessage(JsMsg_TraceDate_Error);
        _ShowJsErrMessageBox();
        return false;
    }
    else {
        if (((endTraceDate - startTraceDate) <= timeInterval * 60 * 60 * 1000) === false) {
            _AddJsErrMessage(JsMsg_TraceDateRange_Error);
            _ShowJsErrMessageBox();
            return false;
        }
    }

    return true;
}

function _RequiredValidation() {
    var modeSelected = $('#SearchType option:selected').val();
    if (!(modeSelected === "A" && $('#UserID').val() !== '' && $('#SessionID').val() !== '' && $('#RequestSessionID').val() !== '') &&
        !(modeSelected === "B" && $('#SysID').val() !== '' && $('#UserID').val() !== '') &&
        !(modeSelected === "C" && $('#SysID').val() !== '' && $('#ControllerName').val() !== '' && $('#ActionName').val() !== '')
    ) {
        _AddJsErrMessage(JsMsg_Required_Error);
        _ShowJsErrMessageBox();
        return false;
    }
    return true;
}