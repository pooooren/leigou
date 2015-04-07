�������

http://api.map.baidu.com/lbsapi/getpoint/index.html


121.564039,31.210602
121.56403,31.209954
���74��

0.002=230��
0.002=200��ģ��

2�����ģ��


http://localhost/rest/rest/query/address/1

http://localhost/rest/rest/query1/address/1?lng=121.463500&lat=31.110255&limit=3
121.463500 31.110255

��õ�ǰ��ĸ���
http://localhost/rest/rest/query2/address/1?lngy=121.463500&laty=31.110255&lng=121.473500&lat=31.120255

��ñ߽����
http://localhost/rest/rest/query3/address/1?lngy=121.5635&laty=31.210255&lat1=31.214764&lng1=121.561075&lat2=31.206255&lng2=121.5675


 alert("point:" + (bounds.ne+bounds.oe)/2 + " " + (bounds.se  + bounds.te)/2);
 
 
 
 http://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
 /**
* ���������������㣬���߾����ĸ��ӡ�
* @see http://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
*/
public static function determineTouchedTiles(p0:Point, p1:Point):Vector.
{
var touched:Vector.=new Vector.();

var x0:Number=p0.x;
var y0:Number=p0.y;
var x1:Number=p1.x;
var y1:Number=p1.y;

var steep:Boolean=Math.abs(y1 - y0) > Math.abs(x1 - x0);
if (steep)
{
x0=p0.y;
y0=p0.x;
x1=p1.y;
y1=p1.x;
}

if (x0 > x1)
{
var x0_old:Number=x0;
var y0_old:Number=y0;
x0=x1;
x1=x0_old;
y0=y1;
y1=y0_old;
}

var ratio:Number=Math.abs((y1 - y0) / (x1 - x0));
var mirror:int=y1 > y0 ? 1 : -1;

for (var col:int= Math.floor(x0); col < Math.ceil(x1); col++)
{
var currY:Number=y0 + mirror * ratio * (col - x0);

//��һ�񲻽����ӱ߼���
var skip:Boolean = false;
if(col == Math.floor(x0)){
skip = (int(currY) != int(y0));
}

if(!skip){
if (!steep)
{
touched.push(new Point(col, Math.floor(currY)));
}
else
{
touched.push(new Point(Math.floor(currY), col));
}
}

//����б�ʼ����Ƿ��п��
if ((mirror > 0 ? (Math.ceil(currY) - currY) : (currY - Math.floor(currY))) < ratio)
{
var crossY:int = Math.floor(currY) + mirror;

//�ж��Ƿ񳬳���Χ
if(crossY>Math.max(int(y0), int(y1)) || crossY

//���߸���
if (!steep)
{
touched.push(new Point(col, crossY));
}
else
{
touched.push(new Point(crossY, col));
}
}
}


return touched;
}