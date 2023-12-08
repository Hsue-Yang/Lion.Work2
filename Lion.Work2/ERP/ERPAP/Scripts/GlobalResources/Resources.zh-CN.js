var _CloseJsErrMessage = "关闭";
var _JsErrMessageBoxTitle = "错误";
var _JsErrMessageScriptOrOnEvent = "不可输入Script标签或是标签内有On事件";
var _PagerMessage = { StrPager1: "共<b>{0}</b>笔, 每页", StrPager2: "笔, 分<b>{0}</b>页, 第", StrPager3: "页" };
var _Pager = { First: "首页", Prev: "上页", Next: "下页", Last: "末页" };
var _Symbol = { First: "<<", Prev: "<", Next: ">", Last: ">>" };
var _StrPager = { StrPager1: "共", StrPager2: "笔，每页", StrPager3: "笔，目前第", StrPager4: "页，共", StrPager5: "页" };
var _PageSizeTitle = "每页几笔";
var _Currently = "目前";
var _Least = "至少";
var _Most = "最多";
var _JsMsg_UpdateSysFunToolError = "更新失败";
var _JsMsg_ToolNoIsRequired = "请先选择条件";
var _JsMsg_CopySysFunToolError = "复制失败";
var _JsMsg_DeleteSysFunToolError = "删除失败";
var _JsMsg_UpdateNameSysFunToolError = "更新名称失败";
var _JsMsg_WorkFlowAPIExecuteSuccess = "执行成功";
var _JsMsg_WorkFlowAPIBackToNodeError = "退回上一节点失败";
var _JsMsg_WorkFlowAPINextToNodeError = "移至下一节点失败";
var _JsMsg_WorkFlowAPIAutoNextToNodeError = "自动移至下一节点失败";
var _JsMsg_WorkFlowAPIAutoBackToNodeError = "自動退回節點失敗";
var _JsMsg_WorkFlowAPITerminateFlowError = "中止流程失败";
var _JsMsg_WorkFlowAPISignatureError = "送出签核结果失败";
var _JsMsg_WorkFlowNextNodeNewUserError = "无法取得下一节点处理人清单";
var _JsMsg_WorkFlowAssignAPIError = "指派处理人API执行失败";
//var _JsMsg_WorkFlowNextNodeNewUserError = "指派处理人不能为空值";
var _JsMsg_WorkFlowAPIPickNewUserError = "成为当前节点处理人失败";
var _JsMsg_CapsLockRemind = "大写锁定中，请留意输入信息的正确性";
var _JsMsg_AjaxMessageLoading = "讯息载入中";
var _JsMsg_AjaxNotFoundMessaged = "无任何讯息";
var _JsMsg_MoreMenuOpen = "展开";
var _JsMsg_MoreMenuClose = "收合";

//#region 取得input 中文說明
function GetInputTypeResources(inputType) {
    var wResources = eval("({InputTypeResource:'',InputTypeCodeResource:'',Required:'必须输入',Select:'必须选取'})");
    switch (inputType) {
        case "TextBoxAlphanumeric":
            {
                wResources.InputTypeResource = "英数字";
                wResources.InputTypeCodeResource = "码";
                break;
            }
        case "TextBoxInteger":
            {
                wResources.InputTypeResource = "数字";
                wResources.InputTypeCodeResource = "码";
                break;
            }
        case "TextBoxDecimal":
            {
                wResources.InputTypeResource = "数字";
                wResources.InputTypeCodeResource = "码";
                break;
            }
        case "TextBoxNotChinese":
            {
                wResources.InputTypeResource = "英数字";
                wResources.InputTypeCodeResource = "码";
                break; 
            }
        case "TextBoxChar8":
            {
                wResources.InputTypeResource = "日期格式";
                wResources.InputTypeCodeResource = "YYYYMMDD";
                break;
            }
        case "TextBoxPassword": 
            { 
                wResources.InputTypeCodeResource = "码";
                break;
            }
        case "TextBoxDatePicker":
            {
                wResources.InputTypeResource = "日期格式";
                wResources.InputTypeCodeResource = "YYYYMMDD";
                break;
            }
        case "TextBox":
            {
                wResources.InputTypeCodeResource = "字";
                break;
            }
        case "TextBoxNumber":
            {
                wResources.InputTypeResource = "数字";
                wResources.InputTypeCodeResource = "码";
                break;
            }
        case "TextBoxIdNo":
            {
                wResources.InputTypeResource = "身份证字号 ";
                wResources.InputTypeCodeResource = " 码";
                break;
            }
        case "TextBoxInterval":
            {
                wResources.InputTypeResource = "日期格式";
                wResources.InputTypeCodeResource = "YYYYMMDD";
                break;
            }
        case "TextBoxYearMonth":
            {
                wResources.InputTypeResource = "日期格式";
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
                BasicCodeResource = "使用者代码6码";
                break;
            }
        case "Agent":
            {
                BasicCodeResource = "同行公司代码6码";
                break;
            }
        case "Airport":
            {
                BasicCodeResource = "几场代码3码";
                break;
            }
        case "City":
            {
                BasicCodeResource = "城市代码3码";
                break;
            }
        case "Country":
            {
                BasicCodeResource = "国家代码2码";
                break;
            }
        case "Suppliers":
            {
                BasicCodeResource = "供应商代码10码";
                break;
            }
        case "Currency":
            {
                BasicCodeResource = "币别代码3码";
                break;
            }
        case "Erat":
            {
                BasicCodeResource = "汇率";
                break;
            }
        case "Hotel":
            {
                BasicCodeResource = "旅馆代码8码";
                break;
            }
        case "Carr":
            {
                BasicCodeResource = "航空公司代码2码";
                break;
            }
    }
    return BasicCodeResource;
}

//#endregion

//#region 設定日曆中文格式
jQuery(function ($) {
    if ($("input[inputtype=TextBoxDatePicker]").length > 0) {
        $.datepicker.regional['zh-TW'] = {
            closeText: '关闭',
            prevText: '<上月',
            nextText: '下月>',
            currentText: '今天',
            monthNames: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
            monthNamesShort: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
            dayNames: ['星期一', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],
            dayNamesShort: ['周日', '周一', '周二', '周三', '周四', '周五', '周六'],
            dayNamesMin: ['日', '一', '二', '三', '四', '五', '六'],
            dateFormat: 'yy-mm-dd', firstDay: 1,
            showMonthAfterYear: false,
            changeMonth: true,
            changeYear: true,
            isRTL: false
        };
        $.datepicker.setDefaults($.datepicker.regional['zh-TW']);
    }
});
//#endregion

