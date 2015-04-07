
function leigou_grid() { }


//添加覆盖物,x-纬度,y-经度,s-刻度,l-网格数目
leigou_grid.add_overlay = function (map, x, y, s, l) {
    var overlay = [, ];
    for (var i = 0; i < l; i++) {
        for (var j = 0; j < l; j++) {
            var o = this.draw_overlay(x + i * s, y + j * s, x + (i + 1) * s, y + (j - 1) * s, i, j);
            overlay[i * l + j] = o;
            map.addOverlay(o);
            //map.addOverlay(draw_overlay(x + i * s, y + j * s, x + (i + 1) * s, y + (j - 1) * s, i, j));
            //Console.WriteLine(i+ " "+j+" "+(i+1)+" "+(j-1));
        }
    }
    return overlay;
};


//i,j用来编码
leigou_grid.draw_overlay = function (i1, j1, i2, j2, i, j) {
    var pStart = new BMap.Point(i1, j1);
    var pEnd = new BMap.Point(i2, j2);
    var rectangle = new BMap.Polygon([
        new BMap.Point(pStart.lng, pStart.lat),
        new BMap.Point(pEnd.lng, pStart.lat),
        new BMap.Point(pEnd.lng, pEnd.lat),
        new BMap.Point(pStart.lng, pEnd.lat)
    ], { strokeColor: "blue", strokeWeight: 2, strokeOpacity: 0.5 });  //创建矩形

    var code = "0" + i + "0" + j;

    rectangle.addEventListener("click", function (e) {
            var bounds = e.target.getBounds();

            var polygon = e.target;
            polygon.setFillColor("green");
            //alert("point:" + bounds.ne + "," + bounds.oe + " " + bounds.se + "," + bounds.te);
            //alert(code);
            //addlabel(bounds.ne,bounds.oe, bounds.se, bounds.te,code)
    });
    rectangle.addEventListener("rightclick", function (e) { this.showcode(e, code); });
    return rectangle;
};
//清除覆盖物
leigou_grid.remove_overlay = function () {
    map.clearOverlays();
};// JavaScript source code



leigou_grid.addlabel = function (x1, y1, x2, y2, code) {
    var point = new BMap.Point((x1 + x2) / 2, (y1 + y2) / 2);
    var opts = {
        position: point,    // 指定文本标注所在的地理位置
        offset: new BMap.Size(0, 0)    //设置文本偏移量
    }
    var label = new BMap.Label(code, opts);  // 创建文本标注对象
    label.setStyle({
        color: "red",
        fontSize: "12px",
        height: "20px",
        lineHeight: "20px",
        fontFamily: "微软雅黑"
    });
    map.addOverlay(label);
};

leigou_grid.openInfo = function (e, code) {
    var bounds = e.target.getBounds();

    var polygon = e.target;
    polygon.setFillColor("green");
    //alert("point:" + bounds.ne + "," + bounds.oe + " " + bounds.se + "," + bounds.te);
    //alert(code);
    //addlabel(bounds.ne,bounds.oe, bounds.se, bounds.te,code)
};

leigou_grid.showcode = function(e, code) {
    var bounds = e.target.getBounds();
    //alert("point:" + bounds.ne + "," + bounds.oe + " " + bounds.se + "," + bounds.te);
    alert(code);
};