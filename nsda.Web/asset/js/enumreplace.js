(function (w) {
    var enumreplace = {
    };
    enumreplace.replaceeventLevel = function (eventlevel) {
        if (eventlevel == 1) {
            return "A";
        }
        else if (eventlevel == 2) {
            return "B";
        }
        else if (eventlevel == 3) {
            return "C";
        }
        else if (eventlevel == 4) {
            return "D";
        }
        else if (eventlevel == 5) {
            return "E";
        }
    };
    enumreplace.replaceeventType = function (eventtype) {
        if (eventtype == 1)
        {
            return "辩论";
        }
        else if (eventtype == 2) {
            return "演讲";
        }
    };
    enumreplace.replaceeventTypeName = function (eventtypeName) {
        if (eventtypeName == 1) {
            return "原创演讲";
        }
        else if (eventtypeName == 2) {
            return "公共论坛式辩论";
        }
        else if (eventtypeName == 3) {
            return "林肯道格拉斯辩论";
        }
        else if (eventtypeName == 4) {
            return "英国议会制辩论";
        }
    };
    enumreplace.replaceFileType = function (fileType) {
        if (fileType == 1) {//img
            return "/asset/img/source_03.png";
        }
        else if (fileType == 2) {//doc
            return "/asset/img/source_10.png";
        }
        else if (fileType == 3) {//pdf
            return "/asset/img/source_03.png";
        }
        else if (fileType == 4) {
            return "/asset/img/source_16.png";
        } else if (fileType == 5) {
            return "/asset/img/source_14.png";
        }
    };

    enumreplace.replaceoperationStatus = function (operationStatus) {
        if (operationStatus == 1) {
            return "退费申请中";
        }
        else if (operationStatus == 2) {
            return "已退费";
        }
    };
    w.enumreplace = enumreplace;
})(window);