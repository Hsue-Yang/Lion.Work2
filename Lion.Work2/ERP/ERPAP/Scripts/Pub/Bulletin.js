var _formElement;

function BulletinForm_onLoad(formElement) {
    var isFirstLogin = $('#IsFirstLogin').val();

    if (isFirstLogin === 'Y') {
        //_openWin("newwindow", _enumERPAP + "/Pub/Bulletin", "SystemID=INGAP");
    }

    //if (targetPathList !== null) {
    //    $(targetPathList).each(function (idx, el) {
    //        if (el.IsOutSourcing === 'Y') {
    //            _openWin(idx, '/Pub/Bulletin', 'systemID=' + el.SysID + '&targetPath=' + encodeURIComponent(el.TargetPath));
    //        } else {
    //            _openWin(idx, el.TargetPath, null);
    //        }
    //    });
    //}
    
    return true;
}