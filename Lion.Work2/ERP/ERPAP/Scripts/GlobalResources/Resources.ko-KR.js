var _CloseJsErrMessage = "닫기";
var _JsErrMessageBoxTitle = "오류";
var _JsErrMessageScriptOrOnEvent = "스크립트 태그 또는 On 이벤트가 있는 태그에 입력할 수 없습니다.";
var _PagerMessage = { StrPager1: "총 <b>{0}</b>개의 항목, 각 페이지당", StrPager2: "개, <b>{0}</b>페이지,", StrPager3: "" };
var _Pager = { First: "처음", Prev: "이전", Next: "다음", Last: "마지막" };
var _Symbol = { First: "<<", Prev: "<", Next: ">", Last: ">>" };
var _StrPager = { StrPager1: "총 ", StrPager2: "개의 항목. 각 페이지당 ", StrPager3: "개의 항목. 페이지 ", StrPager4: "중 ", StrPager5: "" };
var _PageSizeTitle = "한 페이지에";
var _Currently = "현재 ";
var _Least = "최소 ";
var _Most = "최대";
var _JsMsg_UpdateSysFunToolError = "업데이트 실패";
var _JsMsg_ToolNoIsRequired = "조건을 선택해주세요";
var _JsMsg_CopySysFunToolError = "복사 실패";
var _JsMsg_DeleteSysFunToolError = "삭제 실패";
var _JsMsg_UpdateNameSysFunToolError = "이름 업데이트 실패";
var _JsMsg_WorkFlowAPIExecuteSuccess = "실행 성공";
var _JsMsg_WorkFlowAPIBackToNodeError = "이전 노드로 돌아가기 실패";
var _JsMsg_WorkFlowAPINextToNodeError = "다음 노드로 이동하기 실패";
var _JsMsg_WorkFlowAPIAutoNextToNodeError = "자동으로 다음 노드로 이동하기 실패";
var _JsMsg_WorkFlowAPIAutoBackToNodeError = "자동으로 이전 노드로 돌아가기 실패";
var _JsMsg_WorkFlowAPITerminateFlowError = "플로우 종료 실패";
var _JsMsg_WorkFlowAPISignatureError = "서명 실패";
var _JsMsg_WorkFlowNextNodeNewUserError = "다음 노드의 새로운 사용자 목록 가져오기 실패";
var _JsMsg_WorkFlowAssignAPIError = "다음 노드 사용자 할당 API 실패";
//var _JsMsg_WorkFlowNextNodeNewUserError = "다음 노드 사용자를 할당해야 합니다";
var _JsMsg_WorkFlowAPIPickNewUserError = "현재 노드의 새로운 사용자로 변경하기 실패";
var _JsMsg_CapsLockRemind = "대문자 잠김. 사용자 정보가 올바른지 확인해주세요.";
var _JsMsg_AjaxMessageLoading = "메시지 로딩 중";
var _JsMsg_AjaxNotFoundMessaged = "메시지를 찾을 수 없음";
var _JsMsg_MoreMenuOpen = "열기";
var _JsMsg_MoreMenuClose = "닫기";

//#region 取得input 英文說明
function GetInputTypeResources(inputType) {
    var wResources = eval("({InputTypeResource:'',InputTypeCodeResource:'',Required:'Required',Select:'Required'})");
    switch (inputType) {
        case "TextBoxAlphanumeric":
            {
                wResources.InputTypeResource = "영숫자 ";
                wResources.InputTypeCodeResource = " 코드";
                break;
            }
        case "TextBoxInteger":
            {
                wResources.InputTypeResource = "숫자 ";
                wResources.InputTypeCodeResource = " 코드";
                break;
            }
        case "TextBoxDecimal":
            {
                wResources.InputTypeResource = "숫자 ";
                wResources.InputTypeCodeResource = " 코드";
                break;
            }
        case "TextBoxNotChinese":
            {
                wResources.InputTypeResource = "영숫자 ";
                wResources.InputTypeCodeResource = " 코드";
                break;
            }
        case "TextBoxChar8":
            {
                wResources.InputTypeResource = "날짜 형식 ";
                wResources.InputTypeCodeResource = "YYYYMMDD";
                break;
            }
        case "TextBoxPassword":
            {
                wResources.InputTypeCodeResource = " 코드";
                break;
            }
        case "TextBoxDatePicker":
            {
                wResources.InputTypeResource = "날짜 형식 ";
                wResources.InputTypeCodeResource = "YYYYMMDD";
                break;
            }
        case "TextBox":
            {
                wResources.InputTypeCodeResource = " 코드";
                break;
            }
        case "TextBoxNumber":
            {
                wResources.InputTypeResource = "숫자 ";
                wResources.InputTypeCodeResource = " 코드";
                break;
            }
        case "TextBoxIdNo":
            {
                wResources.InputTypeResource = "주민등록번호 ";
                wResources.InputTypeCodeResource = " 코드";
                break;
            }
        case "TextBoxInterval":
            {
                wResources.InputTypeResource = "날짜 형식 ";
                wResources.InputTypeCodeResource = "YYYYMMDD";
                break;
            }
        case "TextBoxYearMonth":
            {
                wResources.InputTypeResource = "날짜 형식 ";
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
                BasicCodeResource = "사용자 ID 6 자리 코드";
                break;
            }
        case "Agent":
            {
                BasicCodeResource = "대리점 코드 6 자리 코드";
                break;
            }
        case "Airport":
            {
                BasicCodeResource = "공항 코드 3 자리 코드";
                break;
            }
        case "City":
            {
                BasicCodeResource = "도시 코드 3 자리 코드";
                break;
            }
        case "Country":
            {
                BasicCodeResource = "국가 코드 2 자리 코드";
                break;
            }
        case "Suppliers":
            {
                BasicCodeResource = "공급업체 10 자리 코드";
                break;
            }
        case "Currency":
            {
                BasicCodeResource = "통화 3 자리 코드";
                break;
            }
        case "Erat":
            {
                BasicCodeResource = "환율";
                break;
            }
        case "Hotel":
            {
                BasicCodeResource = "호텔 8 자리 코드";
                break;
            }
        case "Carr":
            {
                BasicCodeResource = "항공사 2 자리 코드";
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

