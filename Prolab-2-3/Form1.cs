using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Prolab_2_3
{
    public partial class Form1 : Form
    {
        MySqlConnection baglanti;
        MySqlCommand kmt;
        MySqlCommand kmt1;
        MySqlDataReader dr;

        String tarih = DateTime.Now.ToLongDateString();
        String ay = DateTime.Now.ToString("MMMM");


        public Form1()
        {
            InitializeComponent();
            baglanti = new MySqlConnection("Server=localhost;Database=deneme;user=sumey;Pwd='';uid=root");
            panel7.Visible = false;
            panel13.Visible = false;
            panel16.Visible = false;
            panel17.Visible = false;
            BankaDurumu();
        }
        void BankaDurumu()
        {
            double gelir = 0, gider = 0, toplam = 0;
            kmt = new MySqlCommand();
            baglanti.Open();
            kmt.Connection = baglanti;
            kmt.CommandText = "SELECT * FROM islemler where onay_durumu='" + "onaylandı" + "'";
            dr = kmt.ExecuteReader();
            while (dr.Read())
            {
                if (dr["islem"].ToString() == "Para Yatırma" || dr["islem"].ToString() == "Kredi Borcu Ödeme")
                {
                    gelir = gelir + Convert.ToDouble(dr["tutar"].ToString());
                    toplam = toplam + Convert.ToDouble(dr["tutar"].ToString());
                }
                if (dr["islem"].ToString() == "Para Cekme" || dr["islem"].ToString() == "Kredi Talebi ")
                {
                    gider = gider + Convert.ToDouble(dr["tutar"].ToString());
                    toplam = toplam + Convert.ToDouble(dr["tutar"].ToString());
                }


            }

            baglanti.Close();

            label61.Text = gelir.ToString();
            label66.Text = gider.ToString();
            label68.Text = ((gelir - gider) / 100).ToString();
            label70.Text = toplam.ToString();
        }

        void Grid1()
        {
            baglanti.Open();
            MySqlDataAdapter listele = new MySqlDataAdapter("Select * from hesaplar where musteri_No='" + musterino.Text + "' ", baglanti);
            DataTable oku = new DataTable();
            listele.Fill(oku);
            dataGridView1.DataSource = oku;
            baglanti.Close();
        }
        void Grid2()
        {
            baglanti.Open();
            MySqlDataAdapter listele1 = new MySqlDataAdapter("Select * from islemler where (tarih LIKE \'%" + (ay + "%\') and musteri_No='" + musterino.Text + "' "), baglanti);
            DataTable oku1 = new DataTable();
            listele1.Fill(oku1);
            dataGridView2.DataSource = oku1;
            baglanti.Close();

        }
        void Grid3()
        {
            baglanti.Open();
            MySqlDataAdapter listele2 = new MySqlDataAdapter("Select * from islemler where temsilci_No='" + temsilci.Text + "'and onay_durumu='" + "onay bekliyor" + "' ", baglanti);
            DataTable oku1 = new DataTable();
            listele2.Fill(oku1);
            dataGridView3.DataSource = oku1;
            baglanti.Close();

        }
        void Grid4()
        {
            baglanti.Open();
            MySqlDataAdapter listele1 = new MySqlDataAdapter("Select * from musteri where temsilci_No='" + temsilci.Text + "' ", baglanti);
            DataTable oku = new DataTable();
            listele1.Fill(oku);
            dataGridView4.DataSource = oku;
            baglanti.Close();

        }
        void Grid5()
        {
            baglanti.Open();
            MySqlDataAdapter listele1 = new MySqlDataAdapter("Select * from islemler where temsilci_No='" + temsilci.Text + "' ", baglanti);
            DataTable oku = new DataTable();
            listele1.Fill(oku);
            dataGridView5.DataSource = oku;
            baglanti.Close();
        }
        void Grid6()
        {
            baglanti.Open();
            MySqlDataAdapter listele1 = new MySqlDataAdapter("Select * from kur_fiyatlari where kur_No !='"+0+"'", baglanti);
            DataTable oku = new DataTable();
            listele1.Fill(oku);
            dataGridView6.DataSource = oku;
            baglanti.Close();
        }
        void Grid7()
        {
            baglanti.Open();
            MySqlDataAdapter listele1 = new MySqlDataAdapter("Select temsilci_No,temsilci_adi,temsilci_maasi from temsilci",baglanti);
            DataTable oku = new DataTable();
            listele1.Fill(oku);
            dataGridView7.DataSource = oku;
            baglanti.Close();
        }
        void Grid8()
        {
            baglanti.Open();
            MySqlDataAdapter listele1 = new MySqlDataAdapter("Select * from islemler where onay_durumu='"+"onaylandı"+"'", baglanti);
            DataTable oku = new DataTable();
            listele1.Fill(oku);
            dataGridView8.DataSource = oku;
            baglanti.Close();
            int i = 0, j = 0, k = 0;

            kmt = new MySqlCommand();
            baglanti.Open();
            kmt.Connection = baglanti;
            kmt.CommandText = "SELECT * FROM islemler ";
            dr = kmt.ExecuteReader();
            while (dr.Read())
            {
                if (dr["islem"].ToString() == "Para Gönderme")
                {
                    i++;
                }
            }
            baglanti.Close();
            string[] kaynak1 = new string[i];
            string[] hedef1 = new string[i];
            int deadlocksayisi = 0;
            int[] islemNo = new int[i];
            i = 0;
            kmt = new MySqlCommand();
            baglanti.Open();
            kmt.Connection = baglanti;
            kmt.CommandText = "SELECT * FROM islemler ";
            dr = kmt.ExecuteReader();
            while (dr.Read())
            {
                if (dr["islem"].ToString() == "Para Gönderme")
                {
                    kaynak1[i] = dr["kaynak"].ToString();
                    hedef1[i] = dr["hedef"].ToString();
                    islemNo[i] =Convert.ToInt32( dr["islem_No"].ToString());

                    i++;
                }

            }
            baglanti.Close();
            int gecici1 = 0;
            label39.Text = "(";
            for (j = 0; j < i; j++)
            {
                for (k = j+1; k < i; k++)
                {
                    if (kaynak1[j] == hedef1[k] && kaynak1[k] == hedef1[j])
                    {
                            label39.Text += islemNo[k] + "-";
                            gecici1 = 1;
                    }
                }
                if (gecici1 == 1)
                {
                    label39.Text += islemNo[j]+")(";
                    deadlocksayisi++;
                }
                gecici1 = 0;
            }
            label39.Text += ":";

            label8.Text = deadlocksayisi.ToString();
            baglanti.Close();
        }
        void Grid9()
        {
            baglanti.Open();
            MySqlDataAdapter listele1 = new MySqlDataAdapter("Select * from musteri_kredi_borcları where (borc_ayi LIKE \'%" + (ay + "%\') and musteri_No='" + musterino.Text + "' LIMIT 1 "), baglanti);
            DataTable oku1 = new DataTable();
            listele1.Fill(oku1);
            dataGridView9.DataSource = oku1;

            baglanti.Close();

            kmt1 = new MySqlCommand();
            baglanti.Open();
            kmt1.Connection = baglanti;
            kmt1.CommandText = "Select * from musteri_kredi_borcları where (borc_ayi LIKE \'%" + (ay + "%\') and musteri_No='" + musterino.Text + "' ");
            dr = kmt1.ExecuteReader();
            if (dr.Read())
            {
                numericUpDown6.Text = dr["borc_no"].ToString();
                label100.Text = dr["ana_para"].ToString();
                baglanti.Close();

            }

            baglanti.Close();

            kmt1 = new MySqlCommand();
            baglanti.Open();
            kmt1.Connection = baglanti;
            kmt1.CommandText = "Select * from musteri_kredi_borcları where musteri_No='" + musterino.Text + "'";
            dr = kmt1.ExecuteReader();

            while (dr.Read())
            {
                if (dr["borc_no"].ToString() != numericUpDown6.Text)
                {
                    if (Convert.ToInt32(dr["borc_no"].ToString()) == (Convert.ToInt32(numericUpDown6.Text) + 1)) { domainUpDown2.Text = dr["borc_ayi"].ToString(); }

                    domainUpDown2.Items.Add(dr["borc_ayi"].ToString());

                }

            }
            baglanti.Close();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Close();
            kmt = new MySqlCommand();
            baglanti.Open();
            kmt.Connection = baglanti;
            kmt.CommandText = "SELECT * FROM musteri where musteri_No='" + textBox1.Text + "'";
            dr = kmt.ExecuteReader();
            if (dr.Read())
            {
                panel7.Visible = true;
                musterino.Text = dr["musteri_No"].ToString();
                adi.Text = dr["musteri_adi"].ToString();
                soyadi.Text = dr["musteri_soyadi"].ToString();
                Tc.Text = dr["musteri_TC"].ToString();
                telefon.Text = dr["musteri_telefon"].ToString();
                eposta.Text = dr["musteri_eposta"].ToString();
                adres.Text = dr["musteri_adres"].ToString();
                musteri_temsilci.Text = dr["temsilci_No"].ToString();
                baglanti.Close();
                Grid1();
                Grid2();
                Grid9();

            }
            else
            {
                MessageBox.Show("Müşteri bulunamadı");
                panel7.Visible = false;
                baglanti.Close();


            }
            kmt = new MySqlCommand();
            baglanti.Open();
            kmt.Connection = baglanti;
            kmt.CommandText = "SELECT * FROM kur_fiyatlari ";
            dr = kmt.ExecuteReader();
            while (dr.Read())
            {
                domainUpDown1.Items.Add(dr["kur_adi"].ToString());

            }
            baglanti.Close();

            label64.Text = textBox35.Text;
            label82.Text = textBox36.Text;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            kmt1 = new MySqlCommand();
            baglanti.Open();
            kmt1.Connection = baglanti;
            kmt1.CommandText = "update musteri set musteri_eposta='" + eposta.Text + "' WHERE musteri_TC='" + Tc.Text + "' ";
            kmt1.ExecuteNonQuery();
            if (kmt1.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("E posta güncellendi.");
            }

            baglanti.Close();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            kmt1 = new MySqlCommand();
            baglanti.Open();
            kmt1.Connection = baglanti;
            kmt1.CommandText = "update musteri set musteri_adres='" + adres.Text + "' WHERE musteri_TC='" + Tc.Text + "' ";
            kmt1.ExecuteNonQuery();
            if (kmt1.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Adres güncellendi.");
            }

            baglanti.Close();
        }
        private void button4_Click_1(object sender, EventArgs e)
        {
            kmt1 = new MySqlCommand();
            baglanti.Open();
            kmt1.Connection = baglanti;
            kmt1.CommandText = "update musteri set musteri_telefon='" + telefon.Text + "' WHERE musteri_TC='" + Tc.Text + "' ";
            kmt1.ExecuteNonQuery();
            if (kmt1.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Telefon güncellendi.");
            }

            baglanti.Close();
        }
        private void ekle_Click(object sender, EventArgs e)
        {
            baglanti.Close();
            if (textBox7.Text != "")
            {

                String mno = musterino.Text;
                String hesapno = textBox7.Text;
                String temsilci = musteri_temsilci.Text;
                int gecici = 0;
                kmt = new MySqlCommand();
                baglanti.Open();
                kmt.Connection = baglanti;
                kmt.CommandText = "SELECT * FROM hesaplar ";
                dr = kmt.ExecuteReader();
                while (dr.Read())
                {
                    if (hesapno == dr["hesap_No"].ToString())
                    {
                        gecici = 1;
                    }
                }
                baglanti.Close();
                if (gecici == 1)
                {
                    MessageBox.Show("Girdiğiniz hesap numarasında hesap vardır.");
                }
                else
                {
                    MessageBox.Show("Hesap Açma talebiniz iletilmiştir.Lütfen bekleyiniz.");

                    baglanti.Close();
                    kmt1 = new MySqlCommand();
                    baglanti.Open();
                    kmt1.Connection = baglanti;
                    kmt1.CommandText = "insert into islemler (musteri_No,temsilci_No,kaynak,hedef,islem,tutar,kaynak_bakiye,hedef_bakiye,tarih,onay_durumu,kaynak_hesap_turu,hedef_hesap_turu) VALUES ('" + mno + "','" + temsilci + "','" + adi.Text + "','" + hesapno + "','" + "Hesap Acma" + "','" + "0" + "','" + "NULL" + "','" + "0" + "','" + tarih + "','" + "onay bekliyor" + "','" + "NULL" + "','" + domainUpDown1.Text + "')";
                    kmt1.ExecuteNonQuery();
                    baglanti.Close();

                    Grid3();
                    Grid2();
                    Grid5();
                    Grid8();
                }
            }
            else
            {
                MessageBox.Show("Hesap No Girilmedi");
            }
            
        }
        private void sil_Click(object sender, EventArgs e)
        {
            baglanti.Close();
            kmt = new MySqlCommand();
            baglanti.Open();
            String bakiye = "";
            kmt.Connection = baglanti;
            kmt.CommandText = "SELECT * FROM hesaplar where hesap_No='" + textBox8.Text + "'";
            dr = kmt.ExecuteReader();
            if (dr.Read())
            {
                if (dr["hesap_bakiye"].ToString() == "0")
                {
                    MessageBox.Show("Hesap Silme talebiniz iletilmiştir.");
                    bakiye = dr["hesap_bakiye"].ToString();
                    String hesapturu = dr["hesap_turu"].ToString();
                    String temsilci = musteri_temsilci.Text;
                    String mno = musterino.Text;
                    String hesapadi = dr["hesap_No"].ToString();
                    baglanti.Close();
                    kmt1 = new MySqlCommand();
                    baglanti.Open();
                    kmt1.Connection = baglanti;
                    kmt1.CommandText = "insert into islemler (musteri_No,temsilci_No,kaynak,hedef,islem,tutar,kaynak_bakiye,hedef_bakiye,tarih,onay_durumu,kaynak_hesap_turu,hedef_hesap_turu) VALUES ('" + mno + "','" + temsilci + "','" + adi.Text + "','" + hesapadi + "','" + "Hesap Silme" + "','" + "0" + "','" + "0" + "','" + "NULL" + "','" + tarih + "','" + "onay bekliyor" + "','" + hesapturu + "','" + "NULL" + "')";
                    kmt1.ExecuteNonQuery();
                    baglanti.Close();
                    Grid3();
                    Grid2();
                }
                else
                {
                    MessageBox.Show("Bakiyeniz 0 olmadığından hesap silinemez");
                    baglanti.Close();
                }
            }
            else
            {
                MessageBox.Show("Hesap Bulunamadı");
                baglanti.Close();
            }
        }
        private void yatir_Click(object sender, EventArgs e)
        {
            baglanti.Close();
            try
            {
                String bakiye = "";
                String hesapturu = "";
                String ad = "";
                double toplam = 0;
                kmt = new MySqlCommand();
                baglanti.Open();
                kmt.Connection = baglanti;
                kmt.CommandText = "SELECT * FROM hesaplar where hesap_No='" + textBox10.Text + "'";
                dr = kmt.ExecuteReader();

                if (dr.Read())
                {
                    bakiye = dr["hesap_bakiye"].ToString();
                    hesapturu = dr["hesap_turu"].ToString();
                    ad = dr["hesap_No"].ToString();
                    toplam = Convert.ToDouble(bakiye) + Convert.ToDouble(textBox11.Text);
                    if (toplam != 0)
                    {
                        baglanti.Close();
                        kmt1 = new MySqlCommand();
                        baglanti.Open();
                        kmt1.Connection = baglanti;
                        kmt1.CommandText = "UPDATE hesaplar set hesap_bakiye='" + toplam + "' WHERE hesap_No='" + textBox10.Text + "' ";
                        if (kmt1.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("Hesaba bakiye eklendi");

                            baglanti.Close();
                            Grid1();
                            baglanti.Open();
                            kmt1.Connection = baglanti;
                            kmt1.CommandText = "insert into islemler (musteri_No,temsilci_No,kaynak,hedef,islem,tutar,kaynak_bakiye,hedef_bakiye,tarih,onay_durumu,kaynak_hesap_turu,hedef_hesap_turu) VALUES ('" + musterino.Text + "','" + musteri_temsilci.Text + "','" + adi.Text + "','" + ad + "','" + "Para Yatırma" + "','" + textBox11.Text + "','" + "NULL" + "','" + bakiye + "','" + tarih + "','" + "onaylandı" + "','" + hesapturu + "','" + "NULL" + "')";
                            kmt1.ExecuteNonQuery();
                            baglanti.Close();
                            Grid5();
                            Grid2();

                        }
                    }
                }
                else
                {
                    MessageBox.Show("Hesap Bulunamadı");
                    baglanti.Close();
                }
            }
            catch
            {
                MessageBox.Show("Girilmedi");
            }
        }
        private void cek_Click(object sender, EventArgs e)
        {
            baglanti.Close();
            try
            {
                String bakiye = "";
                String hesapturu = "";
                String ad = "";
                double cikarma = 0;
                kmt = new MySqlCommand();
                baglanti.Open();
                kmt.Connection = baglanti;
                kmt.CommandText = "SELECT * FROM hesaplar where hesap_No='" + textBox13.Text + "'";
                dr = kmt.ExecuteReader();

                if (dr.Read())
                {

                    bakiye = dr["hesap_bakiye"].ToString();
                    hesapturu = dr["hesap_turu"].ToString();
                    ad = dr["hesap_No"].ToString();

                    cikarma = Convert.ToDouble(bakiye) - Convert.ToDouble(textBox12.Text);
                    if (cikarma >= 0)
                    {
                        baglanti.Close();

                        kmt1 = new MySqlCommand();
                        kmt1.Connection = baglanti;
                        kmt1.CommandText = "UPDATE hesaplar set hesap_bakiye='" + cikarma + "' WHERE hesap_No='" + textBox13.Text + "' ";
                        baglanti.Open();
                        if (kmt1.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("Hesaptan bakiye çekildi.");
                            baglanti.Close();
                            Grid1();
                            baglanti.Open();
                            kmt1.Connection = baglanti;
                            kmt1.CommandText = "insert into islemler (musteri_No,temsilci_No,kaynak,hedef,islem,tutar,kaynak_bakiye,hedef_bakiye,tarih,onay_durumu,kaynak_hesap_turu,hedef_hesap_turu) VALUES ('" + musterino.Text + "','" + musteri_temsilci.Text + "','" + ad + "','" + adi.Text + "','" + "Para Cekme" + "','" + textBox12.Text + "','" + bakiye + "','" + "NULL" + "','" + tarih + "','" + "onaylandı" + "','" + "NULL" + "','" + hesapturu + "')";
                            kmt1.ExecuteNonQuery();
                            baglanti.Close();
                            Grid5();
                            Grid2();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Hesaptaki bakiye miktarı aşıldı. ");
                        baglanti.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Hesap Bulunamadı");
                    baglanti.Close();
                }
            }
            catch
            {
                MessageBox.Show("Girilmedi");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            baglanti.Close();
            panel16.Visible = false;
            kmt = new MySqlCommand();
            baglanti.Open();
            kmt.Connection = baglanti;
            kmt.CommandText = "SELECT * FROM temsilci where temsilci_No='" + textBox4.Text + "'";
            dr = kmt.ExecuteReader();
            if (dr.Read())
            {
                panel13.Visible = true;
                temsilci.Text = dr["temsilci_No"].ToString();
                label103.Text= dr["mudur_no"].ToString();
                baglanti.Close();

                Grid5();
                Grid4();
                Grid3();

            }
            else
            {
                MessageBox.Show("Müşteri Temsilcisi bulunamadı");
                panel13.Visible = false;
                baglanti.Close();
            }
        }
        private void onay_Click(object sender, EventArgs e)
        {
            baglanti.Close();
            kmt = new MySqlCommand();
            baglanti.Open();
            kmt.Connection = baglanti;
            kmt.CommandText = "SELECT * FROM islemler where islem_No='" + numericUpDown1.Text + "'";

            dr = kmt.ExecuteReader();
            if (dr.Read())
            {
                if (dr["islem"].ToString() == "Hesap Acma")
                {
                    String mno = dr["musteri_No"].ToString();
                    String hedef = dr["hedef"].ToString();
                    String hedefhesapturu = dr["hedef_hesap_turu"].ToString();
                   
                    baglanti.Close();
                    int kurno = 0;
                    baglanti.Open();
                    kmt.Connection = baglanti;
                    kmt.CommandText = "SELECT * FROM kur_fiyatlari where kur_adi='" + hedefhesapturu + "'";
                    dr = kmt.ExecuteReader();
                    if (dr.Read())
                    {

                        kurno = Convert.ToInt32(dr["kur_no"].ToString());
                    
                    
                    }
                    baglanti.Close();


                     kmt1 = new MySqlCommand();
                    baglanti.Open();
                    kmt1.Connection = baglanti;
                    kmt1.CommandText = "Update islemler set onay_durumu='" + "onaylandı" + "'where islem_No='" + numericUpDown1.Text + "'";
                    kmt1.ExecuteNonQuery();
                    baglanti.Close();

                    kmt1 = new MySqlCommand();
                    baglanti.Open();
                    kmt1.Connection = baglanti;
                    kmt1.CommandText = "insert into hesaplar (musteri_No,hesap_No,hesap_bakiye,hesap_turu,kur_no) VALUES ('" + mno + "','" + hedef + "','" + "0" + "','" + hedefhesapturu + "','"+ kurno+ "')";
                    if (kmt1.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("İşlem Onaylandı");
                        baglanti.Close();
                        Grid1();
                        Grid2();
                        Grid3();
                        Grid5();
                    }
                    else
                    {
                        MessageBox.Show("Hesap Bulunamadı");
                        baglanti.Close();
                    }

                }
                else if (dr["islem"].ToString() == "Hesap Silme")
                {
                    String hesapadi = dr["hedef"].ToString();
                    baglanti.Close();
                    kmt1 = new MySqlCommand();
                    baglanti.Open();
                    kmt1.Connection = baglanti;
                    kmt1.CommandText = "Update islemler set onay_durumu='" + "onaylandı" + "'where islem_No='" + numericUpDown1.Text + "'";
                    kmt1.ExecuteNonQuery();
                    baglanti.Close();


                    kmt1.Connection = baglanti;
                    kmt1.CommandText = "DELETE FROM hesaplar WHERE hesap_No=('" + hesapadi + "')";
                    baglanti.Open();
                    if (kmt1.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("İşlem Onaylandı");
                        baglanti.Close();
                        Grid1();
                        Grid2();
                        Grid3();
                        Grid5();
                    }
                }
                else if (dr["islem"].ToString() == "Kredi Talebi")
                {
                    baglanti.Close();
                    kmt1 = new MySqlCommand();
                    baglanti.Open();
                    kmt1.Connection = baglanti;
                    kmt1.CommandText = "Update islemler set onay_durumu='" + "onaylandı" + "'where islem_No='" + numericUpDown1.Text + "'";
                    kmt1.ExecuteNonQuery();
                    baglanti.Close();
                    kredi();
                    MessageBox.Show("İşlem Onaylandı");
                    Grid2();
                    Grid3();
                    Grid5();
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox20.Text != "" && textBox21.Text != "" && textBox22.Text != "" && textBox19.Text != "" && textBox18.Text != "" && textBox9.Text != "")
            {


                baglanti.Close();
                int musterisayisi = 0;
                kmt1 = new MySqlCommand();
                baglanti.Open();
                kmt1.Connection = baglanti;
                kmt1.CommandText = "insert into musteri (musteri_adi, musteri_soyadi, musteri_telefon, musteri_adres, musteri_TC, musteri_eposta, temsilci_No,mudur_no) VALUES  ('" + textBox22.Text + "','" + textBox21.Text + "','" + textBox19.Text + "','" + textBox18.Text + "','" + textBox20.Text + "','" + textBox9.Text + "','" + temsilci.Text + "','" + label103.Text + "')";
                if (kmt1.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Müşteri Eklendi");
                    baglanti.Close();
                    Grid4();

                }
                kmt = new MySqlCommand();
                baglanti.Open();
                kmt.Connection = baglanti;
                kmt.CommandText = "SELECT * FROM temsilci where temsilci_No='" + temsilci.Text + "'";
                dr = kmt.ExecuteReader();
                if (dr.Read())
                {
                    musterisayisi = Convert.ToInt32(dr["musteri_sayisi"].ToString()) + 1;
                }
                baglanti.Close();
                kmt1 = new MySqlCommand();
                baglanti.Open();
                kmt1.Connection = baglanti;
                kmt1.CommandText = "Update temsilci set musteri_sayisi='" + musterisayisi + "'where temsilci_No='" + temsilci.Text + "'";
                kmt1.ExecuteNonQuery();
                baglanti.Close();
            }


             else
            {
                MessageBox.Show("Tüm alanları doldurunuz.");
            }

        }
        private void button10_Click(object sender, EventArgs e)
        {
            kmt1 = new MySqlCommand();
            kmt1.Connection = baglanti;
            kmt1.CommandText = "DELETE FROM musteri WHERE musteri_No=('" + numericUpDown2.Text + "')";
            baglanti.Open();
            if (kmt1.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Müşteri Silindi");
                baglanti.Close();
                Grid4();
            }
        }
        private void button11_Click(object sender, EventArgs e)
        {
            baglanti.Close();
            kmt = new MySqlCommand();
            baglanti.Open();
            kmt.Connection = baglanti;
            kmt.CommandText = "SELECT * FROM musteri where musteri_No='" + numericUpDown2.Text + "'";
            dr = kmt.ExecuteReader();
            if (dr.Read())
            {
                panel16.Visible = true;
                textBox26.Text = dr["musteri_TC"].ToString();
                textBox28.Text = dr["musteri_adi"].ToString();
                textBox27.Text = dr["musteri_soyadi"].ToString();
                textBox25.Text = dr["musteri_telefon"].ToString();
                textBox29.Text = dr["musteri_eposta"].ToString();
                textBox24.Text = dr["musteri_adres"].ToString();
            }
            else
            {
                MessageBox.Show("Müşteri Bulunamadı.");
            }
            baglanti.Close();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            kmt1 = new MySqlCommand();
            baglanti.Open();
            kmt1.Connection = baglanti;
            kmt1.CommandText = "update musteri set musteri_eposta='" + textBox29.Text + "' WHERE musteri_TC='" + textBox26.Text + "' ";
            kmt1.ExecuteNonQuery();
            if (kmt1.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("E posta güncellendi.");
            }

            baglanti.Close();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            kmt1 = new MySqlCommand();
            baglanti.Open();
            kmt1.Connection = baglanti;
            kmt1.CommandText = "update musteri set musteri_adres='" + textBox24.Text + "' WHERE musteri_TC='" + textBox26.Text + "' ";
            kmt1.ExecuteNonQuery();
            if (kmt1.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Adres güncellendi.");
            }

            baglanti.Close();
        }
        private void button12_Click(object sender, EventArgs e)
        {
            kmt1 = new MySqlCommand();
            baglanti.Open();
            kmt1.Connection = baglanti;
            kmt1.CommandText = "update musteri set musteri_telefon='" + textBox25.Text + "' WHERE musteri_TC='" + textBox26.Text + "' ";
            kmt1.ExecuteNonQuery();
            if (kmt1.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Telefon güncellendi.");
            }

            baglanti.Close();

        }
        private void button3_Click(object sender, EventArgs e)
        {
            baglanti.Close();
            kmt = new MySqlCommand();
            baglanti.Open();
            kmt.Connection = baglanti;
            kmt.CommandText = "SELECT * FROM banka_muduru where mudur_no='" + textBox6.Text + "'";
            dr = kmt.ExecuteReader();
            if (dr.Read())
            {
                panel17.Visible = true;
                label101.Text = dr["mudur_no"].ToString();
                baglanti.Close();
                Grid6();
                Grid7();
                Grid8();
            }
            else
            {
                panel17.Visible = false;
                MessageBox.Show("Banka Müdürü Bulunamadı");
                baglanti.Close();
            }
            baglanti.Close();
        }
        private void button14_Click(object sender, EventArgs e)
        {
            baglanti.Close();
            kmt1 = new MySqlCommand();
            baglanti.Open();
            kmt1.Connection = baglanti;
            kmt1.CommandText = "update kur_fiyatlari set kur_degeri='" + textBox30.Text + "' WHERE kur_no='" + numericUpDown3.Text + "' ";
            kmt1.ExecuteNonQuery();
            if (kmt1.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Güncellendi.");
                baglanti.Close();
                Grid6();
            }
            else
            {
                MessageBox.Show("Bulunamadı");
                baglanti.Close();
            }
        }
        private void button15_Click(object sender, EventArgs e)
        {

            kmt = new MySqlCommand();
            baglanti.Open();
            kmt.Connection = baglanti;
            kmt.CommandText = "SELECT * FROM kur_fiyatlari where kur_adi='" + textBox31.Text + "'";
            dr = kmt.ExecuteReader();
            if (dr.Read())
            {
                MessageBox.Show("Kur Zaten Var!");
            }
            else
            {

                baglanti.Close();
                kmt1 = new MySqlCommand();
                baglanti.Open();
                kmt1.Connection = baglanti;
                kmt1.CommandText = "insert into kur_fiyatlari (kur_Adi,kur_degeri) VALUES  ('" + textBox31.Text + "','" + textBox32.Text + "')";
                if (kmt1.ExecuteNonQuery() == 1)
                {

                    MessageBox.Show("Kur Eklendi");
                    baglanti.Close();
                    Grid6();

                }
                baglanti.Close();
                kmt = new MySqlCommand();
                baglanti.Open();
                kmt.Connection = baglanti;
                kmt.CommandText = "SELECT * FROM kur_fiyatlari where kur_adi='" + textBox31.Text + "'";
                dr = kmt.ExecuteReader();
                if (dr.Read())
                {
                    domainUpDown1.Items.Add(dr["kur_adi"].ToString());

                }
                baglanti.Close();
            }
        }
        private void button16_Click(object sender, EventArgs e)
        {
            baglanti.Close();
            kmt1 = new MySqlCommand();
            baglanti.Open();
            kmt1.Connection = baglanti;
            kmt1.CommandText = "update temsilci set temsilci_maasi='" + textBox33.Text + "' WHERE temsilci_No='" + numericUpDown4.Text + "' ";
            kmt1.ExecuteNonQuery();
            if (kmt1.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Maaş güncellendi.");
            }

            baglanti.Close();

        }
        private void button17_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sistem bir ay ileriye alındı.");
            DateTime ayi = Convert.ToDateTime(tarih);
            ayi = ayi.AddMonths(1);
            ay = ayi.ToString("MMMM");
            tarih = ayi.ToLongDateString();
            DateTime gecenayi = Convert.ToDateTime(tarih);
            gecenayi = gecenayi.AddMonths(-1);
            String gecenay = "";
            gecenay = gecenayi.ToString("MMMM");

            kmt1 = new MySqlCommand();
            baglanti.Open();
            kmt1.Connection = baglanti;
            kmt1.CommandText = "Select * from musteri_kredi_borcları where musteri_No='" + musterino.Text + "' LIMIT 2";
            dr = kmt1.ExecuteReader();
            int degisken = 0;
            String gecenayborcu = "";

            while (dr.Read())
            {
                if (dr["borc_ayi"].ToString() == gecenay)
                {
                    degisken = 1;
                    gecenayborcu = dr["kalan_borc"].ToString();
                }

            }
            baglanti.Close();
            double gecikmefaizi = 0;
            double gecikmelikalan = 0;

            if (gecenayborcu != "" ){
                gecikmefaizi = Convert.ToDouble(gecenayborcu) * (Convert.ToDouble(label82.Text) / 100);
                gecikmelikalan = Convert.ToDouble(gecenayborcu) + gecikmefaizi;
            }
            if (degisken == 1)
            {
                kmt1 = new MySqlCommand();
                baglanti.Open();
                kmt1.Connection = baglanti;
                kmt1.CommandText = "Select * from musteri_kredi_borcları where musteri_No='" + musterino.Text + "' LIMIT 2";
                dr = kmt1.ExecuteReader();
                if (dr.Read())
                {
                    baglanti.Close();
                    kmt = new MySqlCommand();
                    baglanti.Open();
                    kmt.Connection = baglanti;
                    kmt.CommandText = "update musteri_kredi_borcları set kalan_borc='" + gecikmelikalan + "' WHERE borc_ayi='" + ay + "' ";
                    kmt.ExecuteNonQuery();
                    baglanti.Close();
                }
            }
            BankaDurumu();
            Grid2();
            Grid7();
            Grid9();

        }
        private void button18_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Değiştirildi");
            label64.Text = textBox35.Text;
            label82.Text = textBox36.Text;
        }
        private void button19_Click(object sender, EventArgs e)
        {
            if (textBox40.Text != "" && textBox41.Text != "" && textBox42.Text != "" && textBox37.Text != "" && textBox38.Text != "" &&  textBox39.Text != "")
            {
                int temsilcino = 0;
                kmt1 = new MySqlCommand();
                baglanti.Open();
                kmt1.Connection = baglanti;
                kmt1.CommandText = "SELECT * from temsilci Order by musteri_sayisi ASC LIMIT 1";
                dr = kmt1.ExecuteReader();
                if (dr.Read())
                {
                    temsilcino = Convert.ToInt32(dr["temsilci_No"].ToString());
                    baglanti.Close();
                }
                int musterisayisi = 0;
                kmt1 = new MySqlCommand();
                baglanti.Open();
                kmt1.Connection = baglanti;
                kmt1.CommandText = "insert into musteri (musteri_adi, musteri_soyadi, musteri_telefon, musteri_adres, musteri_TC, musteri_eposta, temsilci_No,mudur_no) VALUES  ('" + textBox42.Text + "','" + textBox41.Text + "','" + textBox39.Text + "','" + textBox38.Text + "','" + textBox40.Text + "','" + textBox37.Text + "','" + temsilcino.ToString() + "','" + label101.Text + "')";
                if (kmt1.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Müşteri Eklendi.En düşük müşterisi olan '" + temsilcino.ToString() + "' nolu müşteri temsilcisine atandı. ");
                    textBox42.Text = null;
                    textBox41.Text = null;
                    textBox40.Text = null;
                    textBox39.Text = null;
                    textBox38.Text = null;
                    textBox37.Text = null;
                    baglanti.Close();
                    Grid4();
                }
                kmt = new MySqlCommand();
                baglanti.Open();
                kmt.Connection = baglanti;
                kmt.CommandText = "SELECT * FROM temsilci where temsilci_No='" + temsilcino.ToString() + "'";
                dr = kmt.ExecuteReader();
                if (dr.Read())
                {
                    musterisayisi = Convert.ToInt32(dr["musteri_sayisi"].ToString()) + 1;

                }
                baglanti.Close();
                kmt1 = new MySqlCommand();
                baglanti.Open();
                kmt1.Connection = baglanti;
                kmt1.CommandText = "Update temsilci set musteri_sayisi='" + musterisayisi + "'where temsilci_No='" + temsilcino.ToString() + "'";
                kmt1.ExecuteNonQuery();
                baglanti.Close();
            }
            else
            {

                MessageBox.Show("Tüm alanları doldurunuz.");
            }
        }
        private void button20_Click(object sender, EventArgs e)
        {
            baglanti.Close();
            baglanti.Open();
            String lm = numericUpDown5.Text;
            MySqlDataAdapter listele1 = new MySqlDataAdapter(String.Format("SELECT * from islemler Order by islem_No DESC LIMIT {0}", lm), baglanti);
            DataTable oku = new DataTable();
            listele1.Fill(oku);
            dataGridView8.DataSource = oku;
            baglanti.Close();

        }
        private void aktar_Click(object sender, EventArgs e)
        {
            baglanti.Close();
            int a = 0, b = 0;
            String tur = "";
            String bakiye = "";
            String hesap1adi = "";
            kmt1 = new MySqlCommand();
            baglanti.Open();
            kmt1.Connection = baglanti;
            kmt1.CommandText = "SELECT * from hesaplar where hesap_No='" + Convert.ToInt32(textBox14.Text) + "'";
            dr = kmt1.ExecuteReader();

            if (dr.Read())
            {
                tur = dr["hesap_turu"].ToString();
                bakiye = dr["hesap_bakiye"].ToString();
                hesap1adi = dr["hesap_No"].ToString();
                baglanti.Close();
            }
            else
            {
                a = 1;
                MessageBox.Show("Çekilecek hesap bulunamadı.");
                baglanti.Close();
            }
            String tur1 = "";
            String bakiye1 = "";
            String hesap2adi = "";
            kmt1 = new MySqlCommand();
            baglanti.Open();
            kmt1.Connection = baglanti;
            kmt1.CommandText = "Select * from hesaplar where hesap_No='" + Convert.ToInt32(textBox15.Text) + "'";
            dr = kmt1.ExecuteReader();
            if (dr.Read())
            {
                tur1 = dr["hesap_turu"].ToString();
                bakiye1 = dr["hesap_bakiye"].ToString();
                hesap2adi = dr["hesap_No"].ToString();
                baglanti.Close();
            }
            else
            {
                b = 1;
                MessageBox.Show("Yatırılacak hesap bulunamadı.");
                baglanti.Close();
            }
            if (a == 0 && b == 0)
            {
                double kalanbakiye = (Convert.ToDouble(bakiye) - Convert.ToDouble(textBox16.Text));
                if (kalanbakiye >= 0)
                {

                    double toplambakiye = 0;
                double cevrilenpara = 0;
                double cevrilenpara1 = 0, isle = 0;
                double aktarilacakmiktar = Convert.ToDouble(textBox16.Text);
                if (tur == tur1)
                {
                    toplambakiye = aktarilacakmiktar + Convert.ToDouble(bakiye1);
                    MessageBox.Show("Aynı hesap türleri eklenmiştir.");

                }
                else
                {
                    if (tur == "TR")

                    {
                        kmt1 = new MySqlCommand();
                        baglanti.Open();
                        kmt1.Connection = baglanti;
                        kmt1.CommandText = "Select * from kur_fiyatlari";
                        dr = kmt1.ExecuteReader();
                        while (dr.Read())
                        {
                            if (tur1 == dr["kur_adi"].ToString())
                            {
                                cevrilenpara = aktarilacakmiktar / Convert.ToDouble(dr["kur_degeri"].ToString());
                                toplambakiye = cevrilenpara + Convert.ToDouble(bakiye1);
                                MessageBox.Show("Çekilecek TR hesabindan '" + dr["kur_adi"].ToString() + "' hesabına çevrilerek eklenmiştir.");
                            }
                        }

                        baglanti.Close();
                    }

                    else if (tur1 == "TR")
                    {
                        kmt1 = new MySqlCommand();
                        baglanti.Open();
                        kmt1.Connection = baglanti;
                        kmt1.CommandText = "Select * from kur_fiyatlari";
                        dr = kmt1.ExecuteReader();
                        while (dr.Read())
                        {
                            if (tur == dr["kur_adi"].ToString())
                            {
                                cevrilenpara = aktarilacakmiktar * Convert.ToDouble(dr["kur_degeri"].ToString());
                                toplambakiye = cevrilenpara + Convert.ToDouble(bakiye1);
                                MessageBox.Show("Çekilecek '" + dr["kur_adi"].ToString() + "' hesabindan TR hesabına çevrilerek eklenmiştir.");
                            }
                        }


                        baglanti.Close();
                    }
                    else
                    {
                        kmt1 = new MySqlCommand();
                        baglanti.Open();
                        kmt1.Connection = baglanti;
                        kmt1.CommandText = "Select * from kur_fiyatlari";
                        dr = kmt1.ExecuteReader();
                        while (dr.Read())
                        {
                            if (tur == dr["kur_adi"].ToString())
                            {
                                cevrilenpara = Convert.ToDouble(dr["kur_degeri"].ToString());
                            }
                            if (tur1 == dr["kur_adi"].ToString())
                            {
                                cevrilenpara1 = Convert.ToDouble(dr["kur_degeri"].ToString());

                            }

                        }
                        baglanti.Close();
                        isle = (cevrilenpara / cevrilenpara1);
                        isle = Math.Round(isle, 2);
                        toplambakiye = (aktarilacakmiktar * isle) + Convert.ToDouble(bakiye1);
                        MessageBox.Show("Çekilecek '" + tur + "' hesabindan '" + tur1 + "' hesabına çevrilerek eklenmiştir.");

                    }
                    baglanti.Close();
                }

                    kmt1 = new MySqlCommand();
                    baglanti.Open();
                    kmt1.Connection = baglanti;
                    kmt1.CommandText = "insert into islemler (musteri_No,temsilci_No,kaynak,hedef,islem,tutar,kaynak_bakiye,hedef_bakiye,tarih,onay_durumu,kaynak_hesap_turu,hedef_hesap_turu) VALUES ('" + musterino.Text + "','" + musteri_temsilci.Text + "','" + hesap1adi + "','" + hesap2adi + "','" + "Para Gönderme" + "','" + textBox16.Text + "','" + bakiye.ToString() + "','" + bakiye1.ToString() + "','" + tarih + "','" + "onaylandı" + "','" + tur.ToString() + "','" + tur1.ToString() + "')";
                    kmt1.ExecuteNonQuery();
                    baglanti.Close();
                    kmt1 = new MySqlCommand();
                    baglanti.Open();
                    kmt1.Connection = baglanti;
                    kmt1.CommandText = "Update hesaplar set hesap_bakiye='" + kalanbakiye.ToString("0.##") + "'where hesap_No='" + Convert.ToInt32(textBox14.Text) + "'";
                    kmt1.ExecuteNonQuery();
                    baglanti.Close();
                    kmt1 = new MySqlCommand();
                    baglanti.Open();
                    kmt1.Connection = baglanti;
                    kmt1.CommandText = "Update hesaplar set hesap_bakiye='" + toplambakiye.ToString("0.##") + "'where hesap_No='" + Convert.ToInt32(textBox15.Text) + "'";
                    kmt1.ExecuteNonQuery();
                    baglanti.Close();
                    Grid1();
                    Grid2();
                    Grid5();
                    Grid8();
                }
                else
                {
                    MessageBox.Show("Çekilecek tutar çekilecek hesabın içindeki miktarı aştı. ");

                }
            }
        
        }
        private void ode_Click(object sender, EventArgs e)
        {
            double bakiye = 0;
            kmt1 = new MySqlCommand();
            baglanti.Open();
            kmt1.Connection = baglanti;
            kmt1.CommandText = "Select * from musteri_kredi_borcları where (borc_ayi LIKE \'%" + (ay + "%\') and musteri_No='" + musterino.Text + "' ");
            dr = kmt1.ExecuteReader();
            if (dr.Read())
            {
                bakiye = Convert.ToDouble(dr["aylik_borc"].ToString());
                baglanti.Close();

            }
            kmt1 = new MySqlCommand();
            baglanti.Open();
            kmt1.Connection = baglanti;
            kmt1.CommandText = "insert into islemler (musteri_No,temsilci_No,kaynak,hedef,islem,tutar,kaynak_bakiye,hedef_bakiye,tarih,onay_durumu,kaynak_hesap_turu,hedef_hesap_turu) VALUES ('" + musterino.Text + "','" + musteri_temsilci.Text + "','" + adi.Text + "','" + "Banka" + "','" + "Kredi Borcu Ödeme" + "','" + bakiye + "','" + "NULL" + "','" + label70.Text + "','" + tarih + "','" + "onaylandı" + "','" + "TR" + "','" + "TR" + "')";
            kmt1.ExecuteNonQuery();
            baglanti.Close();

            MessageBox.Show("Borcunuz odendi.");
            kmt1 = new MySqlCommand();
            baglanti.Open();
            kmt1.Connection = baglanti;
            kmt1.CommandText = "DELETE FROM musteri_kredi_borcları WHERE borc_no=('" + numericUpDown6.Text + "')";
            kmt1.ExecuteNonQuery();
            baglanti.Close();
            Grid9();
            Grid5();
            Grid2();
            Grid8();

        }
        double anapara = 0, faizoranı = 0, toplam = 0, heray = 0, faiz = 0;
        private void button21_Click(object sender, EventArgs e)
        {
            kmt1 = new MySqlCommand();
            baglanti.Open();
            kmt1.Connection = baglanti;
            kmt1.CommandText = "DELETE FROM musteri_kredi_borcları WHERE musteri_no=('" + musterino.Text + "') and borc_Ayi=('" + domainUpDown2.Text + "')";
            kmt1.ExecuteNonQuery();
            MessageBox.Show("'" + domainUpDown2.Text + "' borcunuz ödenmiştir.");
            baglanti.Close();
        }
        private void button22_Click(object sender, EventArgs e)
        {
            baglanti.Close();
            kmt1 = new MySqlCommand();
            baglanti.Open();
            kmt1.Connection = baglanti;
            kmt1.CommandText = "DELETE FROM musteri_kredi_borcları WHERE musteri_no=('" + musterino.Text + "')";
            kmt1.ExecuteNonQuery();
            MessageBox.Show("Tüm borcunuz ödenmiştir.");
            baglanti.Close();
            Grid9();
        }
        int ay1 = 0;
        DateTime ayi;
        private void button13_Click(object sender, EventArgs e)

        {
            baglanti.Close();
            anapara = Convert.ToDouble(textBox23.Text);
            ay1 = Convert.ToInt32(textBox34.Text);
            faizoranı = Convert.ToDouble(label64.Text);
            ayi = Convert.ToDateTime(tarih);
            faiz = (anapara * (faizoranı / 100));
            toplam = anapara + faiz * ay1;
            heray = toplam / ay1;
            kmt1 = new MySqlCommand();
            baglanti.Open();
            kmt1.Connection = baglanti;
            kmt1.CommandText = "insert into islemler (musteri_No,temsilci_No,kaynak,hedef,islem,tutar,kaynak_bakiye,hedef_bakiye,tarih,onay_durumu,kaynak_hesap_turu,hedef_hesap_turu) VALUES ('" + musterino.Text + "','" + musteri_temsilci.Text + "','" + "Banka" + "','" + adi.Text + "','" + "Kredi Talebi" + "','" + textBox23.Text + "','" + label70.Text + "','" + "NULL" + "','" + tarih + "','" + "onay bekliyor" + "','" + "TR" + "','" + "TR" + "')";
            kmt1.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kredi Talebiniz Gönderildi.");
            Grid3();
            Grid2();
        }
        void kredi()
        {
            baglanti.Close();
            int i = 0;
            kmt1 = new MySqlCommand();
            baglanti.Open();
            kmt1.Connection = baglanti;
            for (i = 1; i <= ay1; i++)
            {
                toplam = toplam - heray;
                toplam = Math.Round(toplam, 2);
                if (toplam < 0) { toplam = 0; }
                kmt1.CommandText = "insert into musteri_kredi_borcları (musteri_no,borc_ayi,ana_para,odenen_faiz,aylik_borc,kalan_borc) Values ('" + musterino.Text + "','" + ayi.ToString("MMMM") + "','" + (heray - faiz).ToString("0.##") + "','" + faiz + "','" + heray.ToString("0.##") + "','" + toplam + "')";
                kmt1.ExecuteNonQuery();
                ayi = ayi.AddMonths(1);
            }
            baglanti.Close();
            Grid9();
        }
    }
    }




 
   





