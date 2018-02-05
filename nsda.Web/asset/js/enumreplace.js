(function (w) {
    var enumreplace = {
    };
    enumreplace.replaceGrade = function (grade) {
        if (grade == 0) {
            return "学前";
        }
        else if (grade == 1) {
            return "1年级";
        }
        else if (grade == 2) {
            return "2年级";
        }
        else if (grade == 3) {
            return "3年级";
        }
        else if (grade == 4) {
            return "4年级";
        }
        else if (grade == 5) {
            return "5年级";
        }
        else if (grade == 6) {
            return "6年级";
        }
        else if (grade == 7) {
            return "7年级";
        }
        else if (grade == 8) {
            return "8年级";
        }
        else if (grade == 9) {
            return "9年级";
        }
        else if (grade == 10) {
            return "10年级";
        }
        else if (grade == 11) {
            return "11年级";
        }
        else if (grade == 12) {
            return "12年级";
        }
        else if (grade == 13) {
            return "13年级";
        }
    };
    enumreplace.replaceGender = function (gender) {
        if (gender == 0) {
            return "未知";
        }
        else if (gender == 1) {
            return "男";
        }
        else if (gender == 2) {
            return "女";
        }
    };
    enumreplace.replaceEventStatus = function (eventStatus) {
        if (eventStatus == 1) {
            return "审核中";
        }
        else if (eventStatus == 2) {
            return "拒绝";
        }
        else if (eventStatus == 3) {
            return "报名中";
        }
        else if (eventStatus == 4) {
            return "停止报名";
        }
        else if (eventStatus == 5) {
            return "比赛中";
        }
        else if (eventStatus == 6) {
            return "比赛完成";
        }
    };
    enumreplace.replacesignUpStatus = function (signUpStatus) {
        if (signUpStatus == 1) {
            return "待确认组队";
        }
        else if (signUpStatus == 2) {
            return "待付款";
        }
        else if (signUpStatus == 3) {
            return "已付款";
        }
        else if (signUpStatus == 4) {
            return "报名成功";
        }
        else if (signUpStatus == 5) {
            return "退赛申请中";
        }
        else if (signUpStatus == 6) {
            return "已退赛";
        }
        else if (signUpStatus == 7) {
            return "组队失败";
        }
        else if (signUpStatus == 8) {
            return "已完成";
        }
    };
    enumreplace.replacezsignUpStatus = function (signUpStatus) {
        if (signUpStatus == 1) {
            return "确认组队";
        }
        else if (signUpStatus == 2) {
            return "待付款";
        }
        else if (signUpStatus == 3) {
            return "已付款";
        }
        else if (signUpStatus == 4) {
            return "报名成功";
        }
        else if (signUpStatus == 5) {
            return "退赛申请中";
        }
        else if (signUpStatus == 6) {
            return "已退赛";
        }
        else if (signUpStatus == 7) {
            return "组队失败";
        }
        else if (signUpStatus == 8) {
            return "已完成";
        }
    };
    enumreplace.replacebsignUpStatus = function (signUpStatus) {
        if (signUpStatus == 1) {
            return "报名邀请中";
        }
        else if (signUpStatus == 2) {
            return "待付款";
        }
        else if (signUpStatus == 3) {
            return "已付款";
        }
        else if (signUpStatus == 4) {
            return "报名成功";
        }
        else if (signUpStatus == 5) {
            return "退赛申请中";
        }
        else if (signUpStatus == 6) {
            return "已退赛";
        }
        else if (signUpStatus == 7) {
            return "组队失败";
        }
        else if (signUpStatus == 8) {
            return "已完成";
        }
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