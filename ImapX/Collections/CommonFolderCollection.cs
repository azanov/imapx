using System.Linq;
using ImapX.Flags;

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

        public CommonFolderCollection(ImapClient client) : base(client) { }

        internal void TryBind(ref Folder folder)
        {
            if (folder.Flags.Contains(FolderFlags.All) || folder.Flags.Contains(FolderFlags.XAllMail))
                All = folder;
            else if (folder.Flags.Contains(FolderFlags.Archive))
                Archive = folder;
            else if( folder.Flags.Contains(FolderFlags.XInbox))
                Inbox = folder;
            else if (folder.Flags.Contains(FolderFlags.Drafts))
                Drafts = folder;
            else if (folder.Flags.Contains(FolderFlags.XImportant))
                Important = folder;
            else if (folder.Flags.Contains(FolderFlags.Flagged) || folder.Flags.Contains(FolderFlags.XStarred))
                Flagged = folder;
            else if (folder.Flags.Contains(FolderFlags.Junk) || folder.Flags.Contains(FolderFlags.XSpam))
                Junk = folder;
            else if (folder.Flags.Contains(FolderFlags.Sent))
                Sent = folder;
            else if (folder.Flags.Contains(FolderFlags.Trash))
                Trash = folder;
            else
            {

                switch (folder.Name.ToLower().Trim())
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
                        Inbox = folder;
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
                        Junk = folder;
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
                        Drafts = folder;
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
                        Sent = folder;
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
                        Trash = folder;
                        break;
                }

            }
        }

        
    }
}