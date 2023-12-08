var _formElement;
var nodePositionArray = new Array();
var arrowPositionArray = new Array();

function SystemWorkFlowChartForm_onLoad(formElement) {
    _formElement = formElement;

    GetNodePositionArray();
    GetArrowPositionArray();

    $('#CanvasDIV').find('img').attr('id', 'FlowChart');

    $('#FlowChart').on('mouseover', function (e) {
        $(this).css('cursor', 'pointer');
    });

    $('#FlowChart').on('mousedown', function (e) {
        var result = CalcMousePosition(e);
        var sysID = $('#SysID').val();
        var wfFlowGroupID = $('#WFFlowGroupID').val();
        var wfFlowID = $('#WFFlowID').val();
        var wfFlowVer = $('#WFFlowVer').val();

        if (result.nodeID != undefined) {
            var para = 'SysID=' + sysID +
                           '&WFFlowGroupID=' + wfFlowGroupID +
                           '&WFFlowID=' + wfFlowID +
                           '&WFFlowVer=' + wfFlowVer +
                           '&WFNodeID=' + result.nodeID +
                           '&NodeSeqX=' + result.nodeSeqX +
                           '&NodeSeqY=' + result.nodeSeqY +
                           '&ExecAction=' + result.execAction;
            var objfeatures = { width: window.screen.availWidth, height: window.screen.availWidth, top: 0, left: 0 };

            $.blockUI({ message: $('#dialog_Refresh') });
            _openWin('newwindow', '/Sys/SystemWorkFlowNodeDetail', para, objfeatures);
        }
    });

    $('table.tblsearch #SysID', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: SysID_onChange
    });

    $('table.tblsearch #WFFlowGroupID', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: WFFlowGroupID_onChange
    });

    $('table.tblsearch #WFCombineKey', _formElement).combobox({
        width: 180,
        isRemoveButton: false
    });

    return true;
}

function SysID_onChange(event) {
    if (event.select.val() === '') {
        $('#WFFlowGroupID > option', _formElement).remove();
        $('#WFCombineKey > option', _formElement).remove();
        $('#WFFlowGroupID', _formElement).combobox('SetSelected', '');
        $('#WFCombineKey', _formElement).combobox('SetSelected', '');

        return false;
    }

    $.ajax({
        url: '/Sys/GetSysSystemWorkFlowGroupIDList',
        type: 'POST',
        data: { sysID: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            $('#WFFlowGroupID > option', _formElement).remove();
            $('#WFFlowGroupID', _formElement).combobox('SetSelected', '');
            if (result != null) {
                for (var i = 1; i < result.length; i++) {
                    $('#WFFlowGroupID', _formElement).append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSysSystemWorkFlowGroupIDList);
            _ShowJsErrMessageBox();
        }
    });

    $.ajax({
        url: '/Sys/GetSysUserSystemWorkFlowIDList',
        type: 'POST',
        data: { sysID: event.select.val(), groupID: $('#WFFlowGroupID').val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            $('#WFCombineKey > option', _formElement).remove();
            $('#WFCombineKey', _formElement).combobox('SetSelected', '');
            if (result != null) {
                for (var i = 1; i < result.length; i++) {
                    $('#WFCombineKey', _formElement).append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSysUserSystemWorkFlowIDList);
            _ShowJsErrMessageBox();
        }
    });
    WFCombineKey_onChange(null);
}

function WFFlowGroupID_onChange(event) {
    $.ajax({
        url: '/Sys/GetSysUserSystemWorkFlowIDList',
        type: 'POST',
        data: { sysID: $('#SysID').val(), groupID: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            $('#WFCombineKey > option', _formElement).remove();
            $('#WFCombineKey', _formElement).combobox('SetSelected', '');
            if (result != null) {
                for (var i = 1; i < result.length; i++) {
                    $('#WFCombineKey', _formElement).append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSysUserSystemWorkFlowIDList);
            _ShowJsErrMessageBox();
        }
    });
    WFCombineKey_onChange(null);
}

function WFCombineKey_onChange(srcElement) {
    var combineKey = $('#WFCombineKey').val();

    $('#WFFlowID').val('');
    $('#WFFlowVer').val('');

    if (combineKey == null || combineKey == '') {
        _btnUnblockUI(this, false);
        return false;
    }

    var array = combineKey.split('|');
    if (array.length != 2) {
        _btnUnblockUI(this, false);
        return false;
    } else {
        $('#WFFlowID').val(array[0]);
        $('#WFFlowVer').val(array[1]);
        return true;
    }
}

//----Button----//
function SelectButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function RefreshOKButton_onClick(srcElement) {
    var Result = _FormValidation();

    $('#ExecAction').val(_ActionTypeSelect);
    if (Result) {
        _formElement.submit();
    }
}

function RefreshNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}

//----Private----//
function CalcMousePosition(event) {
    var nodeID;
    var nodeSeqX, nodeSeqY;
    var execAction;

    var nodeWidth = 90;
    var nodeHeight = 80;
    var spacingSize = 20;

    var posX, posY;
    var rect = $('#FlowChart')[0].getBoundingClientRect();

    //取得mousedown當下的座標位置
    posX = event.clientX - rect.left;
    posY = event.clientY - rect.top;

    //比對節點座標清單，符合表示為已存在之節點並帶入節點代碼nodeID
    nodePositionArray.forEach(function (e) {
        if ((posX >= e[1] && posX <= e[3]) &&
            (posY >= e[2] && posY <= e[4])) {
            nodeID = e[0];
            execAction = _ActionTypeUpdate;
        }
    });

    //節點代碼為空，表示為空白節點並推算其座標nodeSeqX及nodeSeqY
    if (nodeID == undefined) {
        nodeID = '';
        nodeSeqX = Math.floor(posX / (nodeWidth + (spacingSize * 2)));
        nodeSeqY = Math.floor(posY / (nodeHeight + (spacingSize * 2)));
        execAction = _ActionTypeAdd;
    }

//    if (nodeID == undefined) {
//        arrowPositionArray.forEach(function (e) {
//            if (e[2] == e[4]) {
//                if (e[3] > e[5]) {
//                    //垂直向上
//                    if ((posX >= e[2] - spacingSize && posX <= e[4] + spacingSize) &&
//                        (posY >= e[5] && posY <= e[3])) {
//                        nodeID = e[0];
//                    }
//                }
//                else if (e[3] < e[5]) {
//                    //垂直向下
//                    if ((posX >= e[2] - spacingSize && posX <= e[4] + spacingSize) &&
//                        (posY >= e[3] && posY <= e[5])) {
//                        nodeID = e[0];
//                    }
//                }
//            }
//            else if (e[3] == e[5]) {
//                //水平
//                if ((posX >= e[2] && posX <= e[4]) &&
//                    (posY >= e[3] - spacingSize && posY <= e[5] + spacingSize)) {
//                    nodeID = e[0];
//                }
//            }
//        });
//    }

    return {
        nodeID: nodeID,
        nodeSeqX: nodeSeqX,
        nodeSeqY: nodeSeqY,
        execAction: execAction
    };
}

function GetNodePositionArray() {
    nodePositionArray = eval($('#NodePosition').val());
}

function GetArrowPositionArray() {
    arrowPositionArray = eval($('#ArrowPosition').val());
}