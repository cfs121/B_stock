using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Parameter;



namespace GDI
{
    public class Print
    {
        int intercept = 4;//字符串末尾截取长度


        /// <summary>
        /// 生成对应货架区域上等比例的货物图形
        /// </summary>
        /// <param name="shelf"> 货架类，具体的货架参数</param>
        /// <param name="coodsOf">货架上商品标号序列</param>
        /// <param name="lengthOf">货架上商品长度序列</param>
        /// <param name="widthOf">货架上商品宽度序列</param>
        /// <returns>返回</returns>
        public Bitmap StoreMap(shelf shelf, PictureBox box, List<string> coodsOf, List<int> lengthOf, List<int> widthOf)
        {
            int pt = Convert.ToInt32(10 * ((decimal)60 / box.Width));//图例字体大小-像素


            string ident = "";

            Point PIdent = new Point();
            Font font = new Font("新宋体", pt, FontStyle.Bold, GraphicsUnit.Pixel);
            int width = box.Width;
            int height = box.Height;
            Bitmap bmp = new Bitmap(width, height);//按照相框尺寸，新建一个图片对象,
            Graphics g = Graphics.FromImage(bmp);//利用该图片对象生成“画板”
            Pen p = new Pen(Color.Black, 2);//画笔
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//优化图片效果
            Graphics graph = Graphics.FromImage(bmp);
            SizeF sizeF = new SizeF();
            if (shelf.ShelfNumber % 2 != 0)//设置不同的底色区分一二层
            {
                g.Clear(Color.GreenYellow);
            }
            else g.Clear(Color.Gold);
            //判断区域在左还是右
            if (shelf.ShelfNumber / 10 % 2 == 0)//在左，既偶数区
            {
                int reLength;//记录对应比例的长度
                int reWidth;//记录对应比例的宽度
                int toLength = 0;//起始位置
                int maxLenght = shelf.Lenght;
                int maxWidth = shelf.Width;
                int mapGap = (Parameter.Class_Parameter.Gap * height) / maxLenght;//计算图上间距
                for (int i = 0; i < coodsOf.Count; i++)
                {
                    if (lengthOf[i] != 0)
                    {
                        ident = coodsOf[i].Substring(coodsOf[i].Length - intercept);//保留字符串的末尾几位
                        reLength = (lengthOf[i] * height) / maxLenght;//方块对应的长度
                        reWidth = (widthOf[i] * width) / maxWidth;
                        Rectangle rect = new Rectangle((width - reWidth) / 2, toLength, reWidth, reLength);

                        g.DrawRectangle(p, rect);//画出矩形
                        g.FillRectangle(new SolidBrush(Color.Aqua), rect);//填充矩形

                        sizeF = graph.MeasureString(ident, font);

                        PIdent.X = (Int32)(width - sizeF.Width) / 2;
                        PIdent.Y = toLength + reLength / 2 - (Int32)(sizeF.Height / 2);//字符串的Y坐标

                        g.DrawString(ident, font, new SolidBrush(Color.Black), PIdent);//画出图示
                        toLength = toLength + reLength + mapGap;
                    }
                    else continue;

                }
                return bmp;
            }
            else//在右，既奇数区
            {
                int reLength;//记录对应比例的长度
                int reWidth;//记录对应比例的宽度
                int toLength = box.Height;//起始位置
                int maxLenght = shelf.Lenght;//最大长度
                int maxWidth = shelf.Width;//最大宽度
                int mapGap = (Parameter.Class_Parameter.Gap * height) / maxLenght;//计算图上间距

                for (int i = 0; i < coodsOf.Count; i++)
                {
                    if (lengthOf[i] != 0)
                    {
                        ident = coodsOf[i].Substring(coodsOf[i].Length - intercept);//保留字符串的末尾几位
                        reLength = (lengthOf[i] * height) / maxLenght;//方块对应的长度
                        reWidth = (widthOf[i] * width) / maxWidth;
                        Rectangle rect = new Rectangle((width - reWidth) / 2, toLength - reLength, reWidth, reLength);

                        g.DrawRectangle(p, rect);
                        g.FillRectangle(new SolidBrush(Color.Aqua), rect);
                        sizeF = graph.MeasureString(ident, font);
                        PIdent.X = (Int32)(width - sizeF.Width) / 2;
                        PIdent.Y = toLength - reLength / 2 - (Int32)(sizeF.Height / 2);//字符串的Y坐标
                        g.DrawString(ident, font, new SolidBrush(Color.Black), PIdent);
                        toLength = toLength - reLength - mapGap;
                    }
                    else continue;

                }
                return bmp;
            }



        }
        /// <summary>
        /// 图形化显示库存，并将目标货物编号用红框标出
        /// </summary>
        /// <param name="shelf"></param>
        /// <param name="coods">目标编号</param>
        /// <param name="coodsOf"></param>
        /// <param name="lengthOf"></param>
        /// <param name="widthOf"></param>
        /// <returns></returns>
        public Bitmap StoreMap(shelf shelf, PictureBox box, string coods, List<string> coodsOf, List<int> lengthOf, List<int> widthOf)
        {
            string ident = "";
            int pt = Convert.ToInt32(10 * ((decimal)60 / box.Width));//图例字体大小-像素


            Point PIdent = new Point();
            Font font = new Font("新宋体", pt, FontStyle.Bold, GraphicsUnit.Pixel);
            int width = box.Size.Width;

            int height = box.Size.Height;
            Bitmap bmp = new Bitmap(width, height);//按照相框尺寸，新建一个图片对象,
            Graphics g = Graphics.FromImage(bmp);//利用该图片对象生成“画板”
            Pen p = new Pen(Color.Black, 2);//画笔
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//优化图片效果
            Graphics graph = Graphics.FromImage(bmp);
            SizeF sizeF = new SizeF();
            if (shelf.ShelfNumber % 2 != 0)//设置不同的底色区分一二层
            {
                g.Clear(Color.GreenYellow);
            }
            else g.Clear(Color.Gold);
            //判断区域在左还是右
            if (shelf.ShelfNumber / 10 % 2 == 0)//在左，既偶数区
            {
                int reLength;//记录对应比例的长度
                int reWidth;//记录对应比例的宽度
                int toLength = 0;//起始位置
                int maxLenght = shelf.Lenght;
                int maxWidth = shelf.Width;
                int mapGap = (Parameter.Class_Parameter.Gap * height) / maxLenght;//计算图上间距
                for (int i = 0; i < coodsOf.Count; i++)
                {
                    if (lengthOf[i] != 0)
                    {
                        //判断是否为目标货物，
                        if (coodsOf[i] == coods)
                        {
                            p.Color = Color.Red;//是目标货物，将画笔改为红色
                            p.Width = 5;
                        }
                        else
                        {
                            p.Color = Color.Black;
                            p.Width = 2;
                        }



                        ident = coodsOf[i].Substring(coodsOf[i].Length - intercept);//保留字符串的末尾几位
                        reLength = (lengthOf[i] * height) / maxLenght;//方块对应的长度
                        reWidth = (widthOf[i] * width) / maxWidth;
                        Rectangle rect = new Rectangle((width - reWidth) / 2, toLength, reWidth, reLength);

                        g.DrawRectangle(p, rect);//画出矩形
                        g.FillRectangle(new SolidBrush(Color.Aqua), rect);//填充矩形
                        sizeF = graph.MeasureString(ident, font);

                        PIdent.X = (Int32)(width - sizeF.Width) / 2;
                        PIdent.Y = toLength + reLength / 2 - (Int32)(sizeF.Height / 2);//字符串的Y坐标
                        g.DrawString(ident, font, new SolidBrush(Color.Black), PIdent);//画出图示
                        toLength = toLength + reLength + mapGap;
                    }
                    else continue;

                }
                return bmp;
            }
            else//在右，既奇数区
            {
                int reLength;//记录对应比例的长度
                int reWidth;//记录对应比例的宽度
                int toLength = box.Height;//起始位置
                int maxLenght = shelf.Lenght;//最大长度
                int maxWidth = shelf.Width;//最大宽度
                int mapGap = (Parameter.Class_Parameter.Gap * height) / maxLenght;//计算图上间距

                for (int i = 0; i < coodsOf.Count; i++)
                {
                    if (lengthOf[i] != 0)
                    {
                        //判断是否为目标货物，
                        if (coodsOf[i] == coods)
                        {
                            p.Color = Color.Red;//是目标货物，将画笔改为红色
                            p.Width = 5;
                        }
                        else
                        {
                            p.Color = Color.Black;
                            p.Width = 2;
                        }

                        ident = coodsOf[i].Substring(coodsOf[i].Length - intercept);//保留字符串的末尾几位
                        reLength = (lengthOf[i] * height) / maxLenght;//方块对应的长度
                        reWidth = (widthOf[i] * width) / maxWidth;
                        Rectangle rect = new Rectangle((width - reWidth) / 2, toLength - reLength, reWidth, reLength);

                        g.DrawRectangle(p, rect);
                        g.FillRectangle(new SolidBrush(Color.Aqua), rect);
                        sizeF = graph.MeasureString(ident, font);
                        PIdent.X = (Int32)(width - sizeF.Width) / 2;
                        PIdent.Y = toLength - reLength / 2 - (Int32)(sizeF.Height / 2);//字符串的Y坐标
                        g.DrawString(ident, font, new SolidBrush(Color.Black), PIdent);
                        toLength = toLength - reLength - mapGap;
                    }
                    else continue;

                }
                return bmp;
            }
        }

        public Bitmap RGVMap(PictureBox box, string coods, int good_lenght, int good_width)
        {
            int pt = Convert.ToInt32(14 * ((decimal)60 / box.Width));//图例字体大小-像素


            string ident = "";

            Point PIdent = new Point();
            Font font = new Font("新宋体", pt, FontStyle.Bold, GraphicsUnit.Pixel);
            int width = box.Width;
            int height = box.Height;
            Bitmap bmp = new Bitmap(width, height);//按照相框尺寸，新建一个图片对象,

            Graphics g = Graphics.FromImage(bmp);//利用该图片对象生成“画板”
            Pen p = new Pen(Color.Black, 2);//画笔
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//优化图片效果
            Graphics graph = Graphics.FromImage(bmp);
            SizeF sizeF = new SizeF();
            Rectangle rect;
            g.Clear(Color.Blue);
            if (coods == "")
            {

                rect = new Rectangle(0, 0, width, height);

                //g.DrawRectangle(p, rect);//画出矩形
                //g.FillRectangle(new SolidBrush(Color.Blue), rect);//填充矩形
                return bmp;
            }


            int reLength;//记录对应比例的长度
            int reWidth;//记录对应比例的宽度
            int toLength = 0;//起始位置
            int maxLenght = 1000;
            int maxWidth = 1000;
            int mapGap = (Parameter.Class_Parameter.Gap * height) / maxLenght;//计算图上间距


            ident = coods.Substring(coods.Length - intercept);//保留字符串的末尾几位
            reLength = (good_lenght * height) / maxLenght;//方块对应的长度
            reWidth = (good_width * width) / maxWidth;
            rect = new Rectangle((width - reWidth) / 2,(height-reLength)/2, reWidth, reLength);

            g.DrawRectangle(p, rect);//画出矩形
            g.FillRectangle(new SolidBrush(Color.Aqua), rect);//填充矩形

            sizeF = graph.MeasureString(ident, font);

            PIdent.X = (Int32)(width - sizeF.Width) / 2;
            PIdent.Y = (Int32)(height-sizeF.Height)/2;//字符串的Y坐标

            g.DrawString(ident, font, new SolidBrush(Color.Black), PIdent);//画出图示
            toLength = toLength + reLength + mapGap;



            return bmp;


        }

        public Bitmap proBar(PictureBox box,int max,int value)
        {
            


          

            Point PIdent = new Point();
            Font font = new Font("新宋体", 15, FontStyle.Bold, GraphicsUnit.Pixel);
            int width = box.Width;
            int height = box.Height;
            Bitmap bmp = new Bitmap(width, height);//按照相框尺寸，新建一个图片对象,
            int barWidth = box.Width * value / max;
            Graphics g = Graphics.FromImage(bmp);//利用该图片对象生成“画板”
            Pen p = new Pen(Color.DarkOrchid, 2);//画笔
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//优化图片效果
            Graphics graph = Graphics.FromImage(bmp);
            
            Rectangle rect=new Rectangle(0,0,barWidth,box.Height);
            g.Clear(Color.Gainsboro);
            g.DrawRectangle(p, rect);//画出矩形
            g.FillRectangle(new SolidBrush(Color.DarkOrchid), rect);//填充矩形
            //SizeF sizeF = new SizeF();
            //string ident = Convert.ToString((value * 100) / max) + "%";
            //sizeF = graph.MeasureString(ident, font);
            return bmp;
        }

    }

}
