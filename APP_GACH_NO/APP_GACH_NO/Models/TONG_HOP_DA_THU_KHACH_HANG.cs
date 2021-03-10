using System;
using System.Collections.Generic;
using System.Text;

namespace APP_GACH_NO.Models
{
    public class TONG_HOP_DA_THU_KHACH_HANG
    {
        public string HOADON_ID { get; set; }
        public int NAM { get; set; }
        int _thang;
        public int THANG
        {
            get { return _thang; }
            set
            {
                _thang = value;
                NOI_DUNG = "Tháng " + _thang + "/" + NAM.ToString();
            }
        }
        public string KHUVUC { get; set; }
        public string TENTRAM { get; set; }
        public string TENLO { get; set; }
        public string MA_KHANG { get; set; }
        public string TEN_KHANG { get; set; }
        public string DTHOAI_KHANG { get; set; }
        public string DCHI_KHANG { get; set; }
        public decimal? DIEN_TTHU { get; set; }
        public decimal? TIEN_DIEN { get; set; }
        public decimal? TIEN_THUE { get; set; }
        public decimal? TONG_TIEN { get; set; }
        public string MATRACUU { get; set; }
        public string SO_HDON { get; set; }
        public string NOI_DUNG { get; set; }
        public string SOCOT { get; set; }
        public bool TRANG_THAI { get; set; }
        public DateTime? NGAY_NOP { get; set; }
        public string TOKEN { get; set; }
    }
}
