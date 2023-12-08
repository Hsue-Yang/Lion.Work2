var _formElement;
var _treeInstance;
var _dirTree = {
    core: {
        multiple: true,
        check_callback: true,
        themes: null,
        data: []
    },
    plugins: [],
    contextmenu: null,
    Resources: {
        UploadFile: window.JsMsg_UploadFile,
        CreateFile: window.JsMsg_CreateFile,
        NewFile: window.JsMsg_NewFile,
        ReName: window.JsMsg_ReName,
        Remove: window.JsMsg_Remove,
        NodeNM_Duplicate: window.JsMsg_NodeNM_Duplicate,
        PleaseChooseFile: JsMsg_PleaseChooseFile,
        FileNameSpecialCharacters: JsMsg_FileNameSpecialCharacters
    }
};

function SystemEDIFlowDirForm_onLoad(formElement) {
    _formElement = formElement;
    document.onkeydown = KeyDown;
    document.oncontextmenu = _ShowContextMenu;
    document.onclick = _HideContextMenu;
    $('#contextmenu li#CreateFile').click(Create_File);
    $('#contextmenu li#ReName').click(Rename_File);
    $('#contextmenu li#Remove').click(Remove_File);

    GetDirTreeData();
}

function Create_File(srcElement) {
    $('#DirTreeOption').val('CREATE_FILE');
    _CreateNewNode('file', _dirTree.Resources.NewFile, 'jstree-file');
}

function Rename_File(srcElement) {
    var nodeID = _treeInstance._data.core.selected[0];
    if (nodeID === undefined) {
        _AddJsErrMessage(_dirTree.Resources.PleaseChooseFile);
        _ShowJsErrMessageBox();
    } else {
        $('#DirTreeOption').val('RENAME');
        _ReName(_treeInstance.get_node(nodeID));
    }
}

function Remove_File(srcElement) {
    var nodeID = _treeInstance._data.core.selected[0];
    if (nodeID === undefined) {
        _AddJsErrMessage(_dirTree.Resources.PleaseChooseFile);
        _ShowJsErrMessageBox();
    } else {
        var nodeObject = _treeInstance.get_node(nodeID);
        $('#DirTreeSelectNodeJsonStr').val(JSON.stringify(nodeObject));
        $('#DirTreeOption').val('REMOVE');

        if (_UpdateServerFile()) {
            _treeInstance.delete_node(nodeObject);
        }
    }
}

function _HideContextMenu(srcElement) {
    $('#contextmenu').hide();
}

function _ShowContextMenu(srcElement) {
    if ($('.jstree-contextmenu').length === 0) {
        var x = srcElement.clientX;
        var y = srcElement.clientY;
        $('#contextmenu').show();

        $('#contextmenu').css({
            left: x,
            top: y
        });

        return false;
    }

    return true;
}

function _CreateRootFolder(srcElement) {
    $('#contextmenu').hide();
    $('#DirTreeOption').val('CREATE');

    var node = _treeInstance.create_node('#',
        {
            text: _dirTree.Resources.NewDir,
            id: 'js-expand' + Math.floor(Math.random() * 1000) + '|' + parent.id,
            type: 'default',
            icon: null
        },
        'last');

    _treeInstance.deselect_all();
    _treeInstance.select_node(node);

    _ReName(node);
}

function KeyDown(srcElement) {
    if (srcElement.keyCode === 113) {
        $('#DirTreeOption').val('RENAME');
    }
}

function GetDirTreeData(srcElement) {
    _dirTree.core.multiple = dirTree.IsMultiple;
    _dirTree.core.check_callback = dirTree.IsCheckCallback;
    _dirTree.core.themes = dirTree.Themes;
    _dirTree.core.data = dirTree.Data;
    _dirTree.plugins = dirTree.Plugins;
    _dirTree = JSON.parse(_ReplaceTreeObjectNM(JSON.stringify(_dirTree)));

    _dirTree.contextmenu = {
        items: function($node) {
            return {
                'Create_File': {
                    'label': _dirTree.Resources.CreateFile,
                    'action': Create_File
                },
                'Rename': {
                    'label': _dirTree.Resources.ReName,
                    'action': Rename_File
                },
                'Remove': {
                    'label': _dirTree.Resources.Remove,
                    'icon': '',
                    'action': Remove_File
                }
            }
        }
    }

    $('#data')
        .on('rename_node.jstree',
        function (e, data) {
            if ($('#DirTreeOption').val() === 'CREATE_FILE' ||
                $('#DirTreeOption').val() === 'RENAME' ||
                data.text !== data.old) {
                var duplicateNodeArr = $(Object.values(_treeInstance._model.data)).filter(function(idx, el) {
                    return el.state.selected === false &&
                        el.parent === data.node.parent &&
                        el.text.toUpperCase() === data.node.text.toUpperCase();
                });

                if (duplicateNodeArr.length > 0) {
                    _AddJsErrMessage(_dirTree.Resources.NodeNM_Duplicate);
                    _ShowJsErrMessageBox();

                    if ($('#DirTreeOption').val() === 'RENAME') {
                        _treeInstance.set_text(data.node, data.old);
                    } else {
                        _treeInstance.delete_node(data.node);
                    }
                } else if (data.node.text.match(/\\|\/|\||\:|\?|\"|\<|\>|\||\*/) !== null) {
                    _treeInstance.set_text(data.node, data.old);
                    _AddJsErrMessage(_dirTree.Resources.FileNameSpecialCharacters);
                    _ShowJsErrMessageBox();
                }else {
                    data.node.old = data.old;
                    $('#DirTreeSelectNodeJsonStr').val(JSON.stringify(data.node));

                    if (_UpdateServerFile()) {

                        _treeInstance.set_id(data.node, Math.floor(Math.random() * 1000) + '|' + data.node.text);
                    }
                }
            }
        })
        .jstree(_dirTree);
    _treeInstance = $('#data').jstree(true);
}

function _CreateNewNode(type, id, icon) {
    var idNum = Math.floor(Math.random() * 1000);
    var node = _treeInstance.create_node('#',
        {
            text: idNum + id,
            id: idNum + '|' + id,
            type: type,
            icon: icon
        });

    _treeInstance.deselect_all();
    _treeInstance.select_node(node);

    _ReName(node);
}

function _ReName(srcElement) {
    _treeInstance.edit(srcElement);
}

function _UpdateServerFile(srcElement) {
    var upDateResult;
    $('#DirTreeJsonStr').val(JSON.stringify(_treeInstance._model.data));

    var data = new FormData();
    data.append('ExecAction', _ActionTypeUpdate);
    data.append('QuerySysID', $('#QuerySysID').val());
    data.append('DirTreeOption', $('#DirTreeOption').val());
    data.append('DirTreeJsonStr', $('#DirTreeJsonStr').val());
    data.append('DirTreeSelectNodeJsonStr', $('#DirTreeSelectNodeJsonStr').val());
    data.append('SystemControllerAction', 'ERPAPSysSystemEDIFlowDir');

    $.ajax({
        url: '/Sys/SystemEDIFlowDir',
        type: 'POST',
        dataType: 'json',
        contentType: false,
        processData: false,
        data: data,
        async: false,
        success: function(result) {
            upDateResult = result.result;
            if (upDateResult === false) {
                _AddJsErrMessage(result.errMsg);
                _ShowJsErrMessageBox();
            }
        }
    });

    return upDateResult;
}

function _ReplaceTreeObjectNM(srcElement) {
    return srcElement.replace(/Variant/g, 'variant')
        .replace(/ID/g, 'id')
        .replace(/Parent/g, 'parent')
        .replace(/Text/g, 'text')
        .replace(/Icon/g, 'icon');
}