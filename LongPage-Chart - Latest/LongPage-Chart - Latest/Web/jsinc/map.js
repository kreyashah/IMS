function putPaginator(itms) {
    var pgs = itms.pages;    

    var sid = "#" + itms.container;
    $(sid).empty();

    if (parseInt(pgs) < 2) return;

    $(sid).append("Go to page:");

    var sobj = $("<select />", { id : itms.paginator, class : itms.class }).change(function () { getMapData(this); }).css({ "margin-left" : "5px", "font-size" : "7.8pt" });

    for (var i = 1; i < (parseInt(pgs) + 1); i++) {
        $("<option />", { value: i.toString(), text: i.toString() }).appendTo(sobj);
    }

    $(sid).append(sobj);
}
/*******************************************************************************************************************/
function contentString(dtrw, i) {
    var dvid = "infvd" + i.toString() + "pop";
    var hrf = window.location.protocol + "//" + window.location.host + "/case.aspx/?" + $("#locate").val() + "=" + dtrw.id; ;

    var s = "<div id=\"" + dvid + "\" style=\"font-size: 7.8pt; font-family: Verdana, Geneva, sans-serif\">";
    s += "<b>Case Number:</b>&nbsp;&nbsp;<a href=\"" + hrf + "\">" + dtrw.case_no + "</a><br />";
    s += "<b>Reported By:</b>&nbsp;&nbsp;" + dtrw.alias + "<br />";
    s += "<b>Date Reported:</b>&nbsp;&nbsp;" + dtrw.date_reported.substring(0, 10) + "<br />";
    s += "<b>Date Incident Occurred:</b>&nbsp;&nbsp;" + (dtrw.incident_date != "" ? dtrw.incident_date.substring(0, 10) : "") + "<br />";
    s += "<b>Province:</b>&nbsp;&nbsp;" + dtrw.province + "<br />";
    s += "<b>District:</b>&nbsp;&nbsp;" + dtrw.district + "<br />";
    s += "<b>Place Name:</b>&nbsp;&nbsp;" + dtrw.place_name + "<br />";
    s += "<b>Incident:</b>&nbsp;&nbsp;" + dtrw.ext_incident_type + "<br />";
    s += "<b>Displacement Status:</b>&nbsp;" + dtrw.ext_displacement_status + "<br />";
    s += "<b>No Individuals Affected:</b>&nbsp;" + dtrw.num_individuals;
    s += "</div>";
    return s;
}
/*******************************************************************************************************************/
function nudgeMarker(latlng) {
    var mn = .999999;
    var mx = 1.000001;

    var rslt = latlng;

    var markers = mcluster.getMarkers();

    for (var i = 0; i < markers.length; i++) {
        var pos = markers[i].getPosition();

        if (latlng.equals(pos)) {
            var nlat = latlng.lat() * (Math.random() * (mx - mn) + mn);
            var nlng = latlng.lng() * (Math.random() * (mx - mn) + mn);

            rslt = new google.maps.LatLng(nlat, nlng);
        }
    }

    return rslt;
}
/*******************************************************************************************************************/
function putMarkers(list) {
    var data = list.data_rows;

    initialize();

    for (var i = 0; i < data.length; i++) {
        var latlng = new google.maps.LatLng(data[i].gps_south, data[i].gps_east);
        latlng = nudgeMarker(latlng);

        var mkr = new google.maps.Marker({
            map: map,
            position: latlng,
            title: "Case " + data[i].case_no,
            content: contentString(data[i], i)
        });

        infwind = new google.maps.InfoWindow;

        google.maps.event.addListener(mkr, 'click', function () {
            infwind.setContent(this.content);
            infwind.open(map, this);
        });

        mcluster.addMarker(mkr);   
    }    
}
/*******************************************************************************************************************/
/*******************************************************************************************************************/
/*******************************************************************************************************************/
/*******************************************************************************************************************/
/*******************************************************************************************************************/