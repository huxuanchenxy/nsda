using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Utilities
{
    public class ExcelHelper<T>
    {
        /// <summary>
        /// 导出到内存流
        /// </summary>
        /// <param name="datas">需要导出的模型列表</param>
        /// <param name="fieldInfies">导出的字段列表信息</param>
        /// <param name="sheetRows">每个工作表的行数</param>
        /// <returns></returns>
        public static MemoryStream ToExcel(List<T> datas, List<ExportFieldInfo> fieldInfies, int sheetRows = 65536)
        {
            //创建Excel文件的对象
            HSSFWorkbook book = new HSSFWorkbook();
            //需要生成工作溥总簿
            int sheetCount = datas.Count / sheetRows + 1;
            int rowCount = datas.Count;
            for (int i = 0; i < sheetCount; i++)
            {
                //添加一个sheet
                ISheet sheet = book.CreateSheet("Sheet" + Convert.ToString(i));
                //给sheet添加第一行的头部标题
                IRow rowTitle = sheet.CreateRow(0);
                for (int k = 0; k < fieldInfies.Count; k++)
                {
                    rowTitle.CreateCell(k).SetCellValue(fieldInfies.ElementAt(k).DisplayName);
                }
                //处理Excel一张工作簿只能放65536行记录的问题
                //因为头部占一行，所以要减1
                for (int j = 0; j < sheetRows - 1; j++)
                {
                    if (datas != null && datas.Count > 0)
                    {
                        //将数据逐步写入sheet各个行
                        IRow rowtemp = sheet.CreateRow(j + 1);
                        int dataIndex = i * (sheetRows - 1) + j;
                        for (int k = 0; k < fieldInfies.Count; k++)
                        {
                            //获取类型
                            Type type = datas[dataIndex].GetType();
                            //获取指定名称的属性
                            System.Reflection.PropertyInfo propertyInfo = type.GetProperty(fieldInfies.ElementAt(k).FieldName);
                            //获取属性值
                            var value = propertyInfo.GetValue(datas[dataIndex], null);
                            switch (fieldInfies.ElementAt(k).DataType)
                            {
                                case DataTypeEnum.Int:
                                    rowtemp.CreateCell(k).SetCellValue(value.ToObjInt());
                                    break;

                                case DataTypeEnum.Float:
                                    rowtemp.CreateCell(k).SetCellValue(value.ToObjDouble());
                                    break;

                                case DataTypeEnum.Double:
                                    rowtemp.CreateCell(k).SetCellValue(value.ToObjDouble());
                                    break;

                                case DataTypeEnum.String:
                                    rowtemp.CreateCell(k).SetCellValue(value == null ? "" : value.ToString());
                                    break;

                                case DataTypeEnum.DateTime:
                                    rowtemp.CreateCell(k).SetCellValue(value == null ? "" : Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss"));
                                    break;

                                case DataTypeEnum.Date:
                                    rowtemp.CreateCell(k).SetCellValue(value == null ? "" : Convert.ToDateTime(value).ToString("yyyy-MM-dd"));
                                    break;

                                default:
                                    break;
                            }
                        }

                    }
                    else
                    {
                        //所有记录循环完成
                        if (i * (sheetRows - 1) + (j + 1) == rowCount + 1)
                        {
                            // 写入到客户端
                            MemoryStream ms = new MemoryStream();
                            book.Write(ms);
                            ms.Seek(0, SeekOrigin.Begin);
                            return ms;
                        }
                    }
                    //所有记录循环完成
                    if (i * (sheetRows - 1) + (j + 1) == rowCount)
                    {
                        // 写入到客户端
                        MemoryStream ms = new MemoryStream();
                        book.Write(ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        return ms;
                    }
                }
            }
            return null;
        }

    }
}
