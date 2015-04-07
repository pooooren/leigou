
function leigou_grid() { }


//��Ӹ�����,x-γ��,y-����,s-�̶�,l-������Ŀ
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


//i,j��������
leigou_grid.draw_overlay = function (i1, j1, i2, j2, i, j) {
    var pStart = new BMap.Point(i1, j1);
    var pEnd = new BMap.Point(i2, j2);
    var rectangle = new BMap.Polygon([
        new BMap.Point(pStart.lng, pStart.lat),
        new BMap.Point(pEnd.lng, pStart.lat),
        new BMap.Point(pEnd.lng, pEnd.lat),
        new BMap.Point(pStart.lng, pEnd.lat)
    ], { strokeColor: "blue", strokeWeight: 2, strokeOpacity: 0.5 });  //��������

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
//���������
leigou_grid.remove_overlay = function () {
    map.clearOverlays();
};// JavaScript source code



leigou_grid.addlabel = function (x1, y1, x2, y2, code) {
    var point = new BMap.Point((x1 + x2) / 2, (y1 + y2) / 2);
    var opts = {
        position: point,    // ָ���ı���ע���ڵĵ���λ��
        offset: new BMap.Size(0, 0)    //�����ı�ƫ����
    }
    var label = new BMap.Label(code, opts);  // �����ı���ע����
    label.setStyle({
        color: "red",
        fontSize: "12px",
        height: "20px",
        lineHeight: "20px",
        fontFamily: "΢���ź�"
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