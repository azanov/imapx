using ImapX.Enums;
using ImapX.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImapX.Collections
{
    public class CommonFolderCollection : FolderCollection
    {
        public Folder All { get; private set; }
        public Folder Archive { get; private set; }
        public Folder Inbox { get; private set; }
        public Folder Drafts { get; private set; }
        public Folder Important { get; private set; }
        public Folder Flagged { get; private set; }
        public Folder Junk { get; private set; }
        public Folder Sent { get; private set; }
        public Folder Trash { get; private set; }
        
        public CommonFolderCollection(ImapClient client, IEnumerable<Folder> items) : base(client, null, items)
        {
            BindRangeInternal(items);
        }

        internal void BindRangeInternal(IEnumerable<Folder> items)
        {
            foreach (var item in items)
                BindInternal(item);
        }

        internal void BindInternal(Folder item)
        {
            switch (item.Type)
            {
                case SpecialFolderType.All:
                    All = item; break;

                case SpecialFolderType.Archive:
                    Archive = item; break;

                case SpecialFolderType.Drafts:
                    Drafts = item; break;

                case SpecialFolderType.Flagged:
                    Flagged = item; break;

                case SpecialFolderType.Important:
                    All = item; break;

                case SpecialFolderType.Inbox:
                    Inbox = item; break;

                case SpecialFolderType.Junk:
                    Junk = item; break;

                case SpecialFolderType.Sent:
                    Sent = item; break;

                case SpecialFolderType.Trash:
                    Trash = item; break;

                default:
                    switch (item.Name.ToLower().Trim())
                    {
                        case "bandeja de entrada":
                        case "boîte de réception":
                        case "caixa de entrada":
                        case "doručená pošta":
                        case "gelen kutusu":
                        case "indbakke":
                        case "inkorgen":
                        case "innboks":
                        case "odebrane":
                        case "inbox":
                        case "posta in arrivo":
                        case "posteingang":
                        case "postvak in":
                        case "recibidos":
                        case "εισερχόμενα":
                        case "входящие":
                        case "受信トレイ":
                        case "收件匣":
                        case "收件箱":
                        case "받은편지함":
                            Inbox = item;
                            break;

                        case "correio electrónico não solicitado":
                        case "correo basura":
                        case "junk":
                        case "lixo":
                        case "nettsøppel":
                        case "nevyžádaná pošta":
                        case "no solicitado":
                        case "ongewenst":
                        case "posta indesiderata":
                        case "skräp":
                        case "spam":
                        case "wiadomości-śmieci":
                        case "önemsiz":
                        case "ανεπιθύμητα":
                        case "спам":
                        case "垃圾邮件":
                        case "垃圾郵件":
                        case "迷惑メール":
                        case "스팸":
                            Junk = item;
                            break;

                        case "borradores":
                        case "bozze":
                        case "brouillons":
                        case "concepten":
                        case "entwürfe":
                        case "kladder":
                        case "koncepty":
                        case "kopie robocze":
                        case "rascunhos":
                        case "taslaklar":
                        case "utkast":
                        case "πρόχειρα":
                        case "черновики":
                        case "下書き":
                        case "drafts":
                        case "draft":
                        case "draftbox":
                        case "草稿":
                        case "임시보관함":
                            Drafts = item;
                            break;

                        case "e-mails enviados":
                        case "enviada":
                        case "enviado":
                        case "gesendet":
                        case "gönderildi":
                        case "inviati":
                        case "posta inviata":
                        case "odeslaná pošta":
                        case "sendt":
                        case "skickat":
                        case "verzonden":
                        case "wysłane":
                        case "sent":
                        case "sent items":
                        case "sentbox":
                        case "sent messages":
                        case "éléments envoyés":
                        case "απεσταλμένα":
                        case "отправленные":
                        case "寄件備份":
                        case "已发送邮件":
                        case "送信済み":
                        case "보낸편지함":
                            Sent = item;
                            break;

                        case "cestino":
                        case "corbeille":
                        case "kosz":
                        case "koš":
                        case "lixeira":
                        case "papelera":
                        case "papierkorb":
                        case "papirkurv":
                        case "papperskorgen":
                        case "prullenbak":
                        case "çöp kutusu":
                        case "κάδος απορριμμάτων":
                        case "корзина":
                        case "ゴミ箱":
                        case "垃圾桶":
                        case "已删除邮件":
                        case "휴지통":
                        case "trash":
                        case "deleted messages":
                        case "deleted":
                        case "deleted items":
                            Trash = item;
                            break;
                    }
                    break;
            }
        }
            

    }
}
