String.prototype.trim = function () {
    return this.replace(/^\s+|\s+$/g, "");
}
/*************************************************************************************************************************/
function extract_Id(s) {
    var vl = null;
    var rgxp = /\d+/g;
    var mtch = s.match(rgxp);

    if (mtch != null)           
        vl = mtch[mtch.length - 1].trim();        

    return vl;
}
/*************************************************************************************************************************/
function getFieldVal(sid, prfx) {
    var vl = "";
    var fld = prfx + sid;

    if (document.getElementById(fld) != null)
        vl = document.getElementById(fld).value;

    return vl;
}
/*************************************************************************************************************************/
function getPeps() {
    var arry = null;
    var obj = document.getElementById("peps");

    if (obj != null) 
        if (obj.value.indexOf(",") != -1) {
            var pplst = obj.value.split(",");            
            for (var i = 0; i < pplst.length; i++)
                pplst[i] = $.trim(pplst[i]);

            arry = pplst;
        }

    return arry;
}
/*************************************************************************************************************************/
function getCaus() {
    var arry = null;
    var obj = document.getElementById("causes");

    if (obj != null) 
        if (obj.value.indexOf(",") != -1) {
            var caulst = obj.value.split(",");            
            for (var i = 0; i < caulst.length; i++)
                caulst[i] = $.trim(caulst[i]);

            arry = caulst;        
        }

    return arry;
}
/*************************************************************************************************************************/
function checkedPeps(cid) {
    var s = "";
    var peps = Array();
    var arry = getPeps();
    var msk = null;

    if (document.getElementById("pepmask") != null)
        msk = document.getElementById("pepmask").value;

    if (arry != null && msk != null && msk != "")        
        for (var i = 0; i < arry.length; i++) {
            var x = msk.replace("{0}", arry[i]);
            x = x.replace("{1}", cid);
            var chk = document.getElementById(x);
            if (chk != null)
                if (chk.type == "checkbox" && chk.checked == true) peps.push(arry[i]);
        }

        if (peps.length != 0) s = peps.join(",");

    return s;
}
/*************************************************************************************************************************/
function checkedNoPeps(cid) {
    var s = "";
    var pepnm = Array();
    var arry = getPeps();
    var msk = null;

    if (document.getElementById("numpepmask") != null)
        msk = document.getElementById("numpepmask").value;

    if (arry != null && msk != null && msk != "")
        for (var i = 0; i < arry.length; i++) {
            var x = msk.replace("{0}", arry[i]);
            x = x.replace("{1}", cid);
            var txt = document.getElementById(x);
            if (txt != null)
                if (/^\d+$/.test(txt.value)) pepnm.push(arry[i].toString() + "." + txt.value);
        }

    if (pepnm.length != 0) s = pepnm.join(",");

    return s;
}
/*************************************************************************************************************************/
function checkedCaus(cid) {
    var s = "";
    var caus = Array();
    var arry = getCaus();
    var msk = null;

    if (document.getElementById("caumask") != null)
        msk = document.getElementById("caumask").value;

    if (arry != null && msk != null && msk != "")
        for (var i = 0; i < arry.length; i++) {
            var x = msk.replace("{0}", arry[i]);
            x = x.replace("{1}", cid);
            var chk = document.getElementById(x);
            if (chk != null)
                if (chk.type == "checkbox" && chk.checked == true) caus.push(arry[i]);
        }

    if (caus.length != 0) s = caus.join(",");

    return s;
}
/*************************************************************************************************************************/
function otherPep(cid) {
    var othr = null;
    var msk = null;
    var vl = null;

    if (document.getElementById("otheroption") != null && document.getElementById("othrpepmask") != null) {
        msk = document.getElementById("othrpepmask").value;
        othr = document.getElementById("otheroption").value;

        var x = msk.replace("{0}", othr);
        x = x.replace("{1}", cid);

        if (document.getElementById(x) != null)
            vl = document.getElementById(x).value;
    }

    return vl;
}
/*************************************************************************************************************************/
function otherCaus(cid) {
    var othr = null;
    var msk = null;
    var vl = null;

    if (document.getElementById("otheroption") != null && document.getElementById("othrcaumask") != null) {
        msk = document.getElementById("othrcaumask").value;
        othr = document.getElementById("otheroption").value;

        var x = msk.replace("{0}", othr);
        x = x.replace("{1}", cid);

        if (document.getElementById(x) != null)
            vl = document.getElementById(x).value;
    }

    return vl;
}
/*************************************************************************************************************************/
function validate_date(sdt) {
    var vl = false;

    var rgxp1 = /^\d{2}\/\d{2}\/\d{4}$/;
    var rgxp2 = /^\d{2}\-\d{2}\-\d{4}$/;

    if (sdt.match(rgxp1) != null)
        vl = true;
    else
        if (sdt.match(rgxp2) != null)
            vl = true;

    return vl;
}
/*************************************************************************************************************************/
function validate_coord(s) {
    var vl = false;

    var rgxp = /^\-{0,1}\d{2}\.\d{1,7}$/;

    vl = Boolean(rgxp.test(s));

    return vl;
}
/*************************************************************************************************************************/
function getClientLeftPart() {
    var loc = window.location.protocol + "//" + window.location.host;
    
    return loc;
}
/*************************************************************************************************************************/
function goHome() {
    var hm = window.location.protocol + "//" + window.location.host + "/default.aspx";
    window.location.href = hm;
}
/*************************************************************************************************************************/
function yearDiff(sfrom, sto) {
    var s = "";

    try {
        var f = new Date(sfrom);
        var t = new Date(sto);

        s = t.getFullYear() - f.getFullYear();
    }
    catch (e) {}

    return s.toString();
}
/*************************************************************************************************************************/
/*************************************************************************************************************************/
$(document).ready(function () {    
    $('input[id*="date_reported"]').datepicker({ dateFormat: "dd/mm/yy", yearRange: "c-15:c+2", changeYear: true });
    $("#verified_on").datepicker({ dateFormat: "dd/mm/yy", yearRange: "c-15:c+2", changeYear: true });
    $("#srch_date_reported").datepicker({ dateFormat: "dd/mm/yy", yearRange: "c-15:c+2", changeYear: true });
    $("#dob").datepicker({ dateFormat: "dd/mm/yy", changeYear: true, yearRange: "c-100:c+1" });
    $("#srch_period_from").datepicker({ dateFormat: "dd/mm/yy", yearRange: "c-15:c+2", changeYear: true });
    $("#srch_period_to").datepicker({ dateFormat: "dd/mm/yy", yearRange: "c-15:c+2", changeYear: true });
    $("#event_date").datepicker({ dateFormat: "dd/mm/yy", yearRange: "c-15:c+2", changeYear: true });
    $("#assist_date").datepicker({ dateFormat: "dd/mm/yy", yearRange: "c-15:c+2", changeYear: true });
    $("#srch_dob").datepicker({ dateFormat: "dd/mm/yy", yearRange: "c-100:c+1", changeYear: true });
    $("#srch_dob").datepicker({ dateFormat: "dd/mm/yy", yearRange: "c-100:c+1", changeYear: true });
    $("#incident_date").datepicker({ dateFormat: "dd/mm/yy", yearRange: "c-15:c+2", changeYear: true });
    $("#survey_date").datepicker({ dateFormat: "dd/mm/yy", yearRange: "c-15:c+2", changeYear: true });
    $("#aq_date").datepicker({ dateFormat: "dd/mm/yy", yearRange: "c-15:c+2", changeYear: true });
    $("#date_displaced").datepicker({ dateFormat: "dd/mm/yy", yearRange: "c-20:c+2", changeYear: true });

    $("#period_from").datepicker({ dateFormat: "dd/mm/yy", yearRange: "c-15:c+2", changeYear: true });
    $("#period_to").datepicker({ dateFormat: "dd/mm/yy", yearRange: "c-15:c+2", changeYear: true });
    $("#srch_period_from").datepicker({ dateFormat: "dd/mm/yy", yearRange: "c-15:c+2", changeYear: true });
    $("#srch_period_to").datepicker({ dateFormat: "dd/mm/yy", yearRange: "c-15:c+2", changeYear: true });
    $("#cmp_from").datepicker({ dateFormat: "dd/mm/yy", yearRange: "c-15:c+2", changeYear: true });
    $("#cmp_to").datepicker({ dateFormat: "dd/mm/yy", yearRange: "c-15:c+2", changeYear: true });

    $("#imghlp").click(function () {
        $("#hlpbx").dialog({ width: 450, height: 600, position: { my: "right", at: "right" }, title: "Help" });
    });        
});
/*************************************************************************************************************************/
/*************************************************************************************************************************/