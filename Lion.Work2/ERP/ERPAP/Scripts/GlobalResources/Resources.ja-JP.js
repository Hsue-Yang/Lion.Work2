var _CloseJsErrMessage = "Close";
var _JsErrMessageBoxTitle = "Error";
var _JsErrMessageScriptOrOnEvent = "Can not enter the Script tag or tags where have On event";
var _PagerMessage = { StrPager1: "Total<b>{0}</b>items, Each page with", StrPager2: "items, of<b>{0}</b>, Page", StrPager3: "" };
var _Pager = { First: "First", Prev: "Prev", Next: "Next", Last: "Last" };
var _Symbol = { First: "<<", Prev: "<", Next: ">", Last: ">>" };
var _StrPager = { StrPager1: "Total ", StrPager2: " items. Each page with ", StrPager3: " items. Page ", StrPager4: " of ", StrPager5: "" };
var _PageSizeTitle = "one page have";
var _Currently = "Currently ";
var _Least = "Least ";
var _Most = "Most";
var _JsMsg_UpdateSysFunToolError = "Update Failed";
var _JsMsg_ToolNoIsRequired = "Please Select Condition";
var _JsMsg_CopySysFunToolError = "Copy Failed";
var _JsMsg_DeleteSysFunToolError = "Delete Failed";
var _JsMsg_UpdateNameSysFunToolError = "Update Name Failed";
var _JsMsg_WorkFlowAPIExecuteSuccess = "Execute success";
var _JsMsg_WorkFlowAPIBackToNodeError = "Back to node failure";
var _JsMsg_WorkFlowAPINextToNodeError = "Next to node failure";
var _JsMsg_WorkFlowAPIAutoNextToNodeError = "Auto next to node failure";
var _JsMsg_WorkFlowAPIAutoBackToNodeError = "Auto back to node failure";
var _JsMsg_WorkFlowAPITerminateFlowError = "Terminate flow failure";
var _JsMsg_WorkFlowAPISignatureError = "Signature failure";
var _JsMsg_WorkFlowNextNodeNewUserError = "Get next node new user list failure";
var _JsMsg_WorkFlowAssignAPIError = "Next node user assign API failure";
//var _JsMsg_WorkFlowNextNodeNewUserError = "Next node user needs to assign";
var _JsMsg_WorkFlowAPIPickNewUserError = "Become current node new user failure";
var _JsMsg_CapsLockRemind = "Caps Lock. Please note user information is correct.";
var _JsMsg_AjaxMessageLoading = "Message Loading";
var _JsMsg_AjaxNotFoundMessaged = "Not Found Messaged";
var _JsMsg_MoreMenuOpen = "Open";
var _JsMsg_MoreMenuClose = "Close";

//#region 取得input 英文說明
function GetInputTypeResources(inputType) {
    var wResources = eval("({InputTypeResource:'',InputTypeCodeResource:'',Required:'Required',Select:'Required'})");
    switch (inputType) {
        case "TextBoxAlphanumeric":
            {
                wResources.InputTypeResource = "Alphanumeric ";
                wResources.InputTypeCodeResource = " Code";
                break;
            }
        case "TextBoxInteger":
            {
                wResources.InputTypeResource = "Number ";
                wResources.InputTypeCodeResource = " Code";
                break;
            }
        case "TextBoxDecimal":
            {
                wResources.InputTypeResource = "Number ";
                wResources.InputTypeCodeResource = " Code";
                break;
            }
        case "TextBoxNotChinese":
            {
                wResources.InputTypeResource = "Alphanumeric ";
                wResources.InputTypeCodeResource = " Code";
                break;
            }
        case "TextBoxChar8":
            {
                wResources.InputTypeResource = "Date Type ";
                wResources.InputTypeCodeResource = "YYYYMMDD";
                break;
            }
        case "TextBoxPassword":
            {
                wResources.InputTypeCodeResource = " Code";
                break;
            }
        case "TextBoxDatePicker":
            {
                wResources.InputTypeResource = "Date Type ";
                wResources.InputTypeCodeResource = "YYYYMMDD";
                break;
            }
        case "TextBox":
            {
                wResources.InputTypeCodeResource = " Code";
                break;
            }
        case "TextBoxNumber":
            {
                wResources.InputTypeResource = "Number ";
                wResources.InputTypeCodeResource = " Code";
                break;
            }
        case "TextBoxIdNo":
            {
                wResources.InputTypeResource = "Idno ";
                wResources.InputTypeCodeResource = " Code";
                break;
            }
        case "TextBoxInterval":
            {
                wResources.InputTypeResource = "Date Type ";
                wResources.InputTypeCodeResource = "YYYYMMDD";
                break;
            }
        case "TextBoxYearMonth":
            {
                wResources.InputTypeResource = "Date Type ";
                wResources.InputTypeCodeResource = "YYYYMM";
            }
    }

    return wResources
}

function GetBasicCodeResources(basicCodeType) {
    var BasicCodeResource = "";
    switch (basicCodeType) {
        case "Staff":
            {
                BasicCodeResource = "User ID 6 Code";
                break;
            }
        case "Agent":
            {
                BasicCodeResource = "Ageng Code 6 Code";
                break;
            }
        case "Airport":
            {
                BasicCodeResource = "Airport Code 3 Code";
                break;
            }
        case "City":
            {
                BasicCodeResource = "City Code 3 Code";
                break;
            }
        case "Country":
            {
                BasicCodeResource = "Country Code 2 Code";
                break;
            }
        case "Suppliers":
            {
                BasicCodeResource = "Suppliers 10 Code";
                break;
            }
        case "Currency":
            {
                BasicCodeResource = "Currency 3 Code";
                break;
            }
        case "Erat":
            {
                BasicCodeResource = "Exchange rate";
                break;
            }
        case "Hotel":
            {
                BasicCodeResource = "Hotel 8 Code";
                break;
            }
        case "Carr":
            {
                BasicCodeResource = "AirLine 2 Code";
                break;
            }

    }

    return BasicCodeResource;
}

//#endregion

//#region 設定小日曆英文格式
jQuery(function ($) {
    if ($("input[inputtype=TextBoxDatePicker]").length > 0) {
        $.datepicker.regional['en-US'] = {
            closeText: 'Close',
            prevText: '<Prev',
            nextText: 'Next>',
            currentText: 'Today',
            monthNames: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            monthNamesShort: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            dayNames: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'friday', 'Sturday'],
            dayNamesMin: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'St'],
            dateFormat: 'yy-mm-dd', firstDay: 1,
            showMonthAfterYear: false,
            changeMonth: true,
            changeYear: true,
            isRTL: false
        };
        $.datepicker.setDefaults($.datepicker.regional['en-US']);
    }
});
//#endregion

