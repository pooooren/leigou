
var overlay = [ ];

function add_overlay(map, lng, lat, code) {

   //var o=draw_overlay(lng,lat,code);
   //overlay[code] = o;
  // map.addOverlay(o);
    //return overlay;
};

function draw1(map) {

    //121.761500, 31.408255, 121.363500, 31.010255
    var pStart = new BMap.Point(121.363500, 31.408255);
    var pEnd = new BMap.Point(121.761500, 31.010255);
    var rectangle = new BMap.Polygon([
        new BMap.Point(pStart.lng, pStart.lat),
        new BMap.Point(pEnd.lng, pStart.lat),
        new BMap.Point(pEnd.lng, pEnd.lat),
        new BMap.Point(pStart.lng, pEnd.lat)
    ], { strokeColor: "blue", strokeWeight: 2, strokeOpacity: 0 });  //创建矩形

    map.addOverlay(rectangle);
}
function draw_overlay(map,lng,lat,code) {
    var pStart = new BMap.Point(lng-0.001, lat+0.001);
    var pEnd = new BMap.Point(lng+0.001, lat-0.001);
    var rectangle = new BMap.Polygon([
        new BMap.Point(pStart.lng, pStart.lat),
        new BMap.Point(pEnd.lng, pStart.lat),
        new BMap.Point(pEnd.lng, pEnd.lat),
        new BMap.Point(pStart.lng, pEnd.lat)
    ], { strokeColor: "blue", strokeWeight: 2, strokeOpacity: 0.5 });  //创建矩形

    map.addOverlay(rectangle);
    rectangle.addEventListener("click", function (e) {
        openInfo(e, code);
    });
    rectangle.addEventListener("dblclick", function (e) {
        //openInfo1(e, code);
    });
    //加上标注
    //addlabel(lng, lat, code);

    rectangle.addEventListener("rightclick", function (e) { showcode(e, code); });
    return rectangle;
};

//清除覆盖物
    function remove_overlay() {
        map.clearOverlays();
    };// JavaScript source code


    function addlabel(lng, lat,code) {
        var point = new BMap.Point(lng, lat);
        var opts = {
            position: point,    // 指定文本标注所在的地理位置
            offset: new BMap.Size(-15, -10)    //设置文本偏移量
        }
        var label = new BMap.Label(code, opts);  // 创建文本标注对象
        label.setStyle({
            color: "red",
            fontSize: "12px",
            height: "20px",
            lineHeight: "20px",
            border: "0",
            backgroud: "0.05",
            fontFamily: "微软雅黑"
        });
        map.addOverlay(label);
    };
    function openInfo(e, code) {
        var bounds = e.target.getBounds();
        var polygon = e.target;
    
        if (polygon.getFillColor() == null || polygon.getFillColor() == 'green') {
            // var new_polygon = new BMap.Polygon(polygon.Fn, { strokeColor: "blue", strokeWeight: 2, strokeOpacity: 0.5 })
            // new_polygon.addEventListener("click", function (e) {
            //     openInfo1(e, code);
            //});
            polygon.setFillColor("");
        } else {
            polygon.setFillColor("green");
        }
        //alert("point:" + bounds.ne + "," + bounds.oe + " " + bounds.se + "," + bounds.te);
        //alert(code);
        //addlabel(bounds.ne,bounds.oe, bounds.se, bounds.te,code)
    };

    function openInfo1(e, code) {
        var polygon = e.target;
        polygon.setFillColor("green");
    };

    function showcode(e, code) {
        var bounds = e.target.getBounds();
        alert("point:" + (bounds.oe  + bounds.te)/2+","+(bounds.ne + bounds.se)/2+  "   code:"+ code);
       // alert(code);
    };


    function getPolygon(grid,array, code) {
        var index = getindex(array, code);
        return grid[index];
    };
    //get the polygon index
    function getindex(array, code) {
        var aa=-1;
        for(var i=0;i<array.length;i++){
            if (array[i] == code) aa = i;
        }
        return aa;
    };