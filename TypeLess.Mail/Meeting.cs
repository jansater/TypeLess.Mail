using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeLess.Mail;

namespace TypeLess.Mail
{
    public class Meeting
    {
        private const string CRLF = "\r\n";
        private const string PRODID = "-//TypeLess.Mail//VCalendar 1.0//EN";
        private string guid;
        private DateTime start;
        private DateTime end;
        private string summary;
        private string location;
        private string description;

        public DateTime Start
        {
            get { return this.start; }
            set { this.start = value; }
        }

        public DateTime End
        {
            get { return this.end; }
            set { this.end = value; }
        }

        public string Summary
        {
            get { return this.summary; }
            set { this.summary = value; }
        }

        public string Location
        {
            get { return this.location; }
            set { this.location = value; }
        }

        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        public Meeting(DateTime start, DateTime end)
        {
            this.start = start;
            this.end = end;
            guid = Guid.NewGuid().ToString().Replace("-", "") + Guid.NewGuid().ToString().Replace("-", "");
        }

        private static string NotNull(string str)
        {
            return str ?? String.Empty;
        }

        private static string FormatDateTime(DateTime dt)
        {
            return String.Format("{0:D4}{1:D2}{2:D2}T{3:D2}{4:D2}{5:D2}", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }

        private static string FormatUtcDateTime(DateTime dt)
        {
            return FormatDateTime(dt) + "Z";
        }

        public string Generate(IEnumerable<Contact> list, Contact from)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("BEGIN:VCALENDAR").Append(CRLF);
            sb.Append("METHOD:REQUEST").Append(CRLF);
            sb.Append("PRODID:").Append(PRODID).Append(CRLF);
            sb.Append("VERSION:2.0").Append(CRLF);

            

            // v-event
            sb.Append("BEGIN:VEVENT").Append(CRLF);
            sb.Append("DTSTAMP:").Append(FormatUtcDateTime(DateTime.UtcNow)).Append(CRLF);
            sb.Append("DTSTART:").Append(FormatUtcDateTime(start)).Append(CRLF);
            sb.Append("SUMMARY:").Append(NotNull(summary)).Append(CRLF);
            sb.Append("UID:").Append(guid).Append(CRLF);
            sb.Append("LOCATION:").Append(NotNull(location)).Append(CRLF);
            if (from != null)
            {
                sb.Append(@"ORGANIZER;CN=").Append(from.Name).Append(@":MAILTO" + ":").Append(from.MailAddress).Append(CRLF);
            }
            foreach (Contact contact in list)
            {
                if (contact.Type == ContactType.Required || contact.Type == ContactType.To)
                {
                    sb.Append(@"ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;RSVP=TRUE;CN="
                        + contact.Name + @":MAILTO" + ":" + contact.MailAddress).Append(CRLF);
                }
            }
            foreach (Contact contact in list)
            {
                if (contact.Type == ContactType.Optional || contact.Type == ContactType.Cc)
                {
                    sb.Append(@"ATTENDEE;ROLE=OPT-PARTICIPANT;PARTSTAT=NEEDS-ACTION;RSVP=TRUE;CN="
                        + contact.Name + @":MAILTO" + ":" + contact.MailAddress).Append(CRLF);
                }
            }
            foreach (Contact contact in list)
            {
                if (contact.Type == ContactType.Resource || contact.Type == ContactType.Bcc)
                {
                    sb.Append(@"ATTENDEE;CUTYPE=RESOURCE;ROLE=NON-PARTICIPANT;PARTSTAT=NEEDS-ACTION;RSVP=TRUE;CN="
                        + contact.Name + @":MAILTO" + ":" + contact.MailAddress).Append(CRLF);
                }
            }
            sb.Append("DTEND:").Append(FormatUtcDateTime(end)).Append(CRLF);
            sb.Append("DESCRIPTION:").Append(NotNull(description)).Append(CRLF);
            sb.Append("SEQUENCE:0").Append(CRLF);
            sb.Append("PRIORITY:5").Append(CRLF);
            sb.Append("CLASS:").Append(CRLF);
            sb.Append("CREATED:").Append(FormatUtcDateTime(DateTime.UtcNow)).Append(CRLF);
            sb.Append("LAST-MODIFIED:").Append(FormatUtcDateTime(DateTime.UtcNow)).Append(CRLF);
            sb.Append("STATUS:CONFIRMED").Append(CRLF);
            sb.Append("TRANSP:OPAQUE").Append(CRLF);
            sb.Append("X-MICROSOFT-CDO-BUSYSTATUS:BUSY").Append(CRLF);
            sb.Append("X-MICROSOFT-CDO-INSTTYPE:0").Append(CRLF);
            sb.Append("X-MICROSOFT-CDO-INTENDEDSTATUS:BUSY").Append(CRLF);
            sb.Append("X-MICROSOFT-CDO-ALLDAYEVENT:FALSE").Append(CRLF);
            sb.Append("X-MICROSOFT-CDO-IMPORTANCE:1").Append(CRLF);
            sb.Append("X-MICROSOFT-CDO-OWNERAPPTID:-611620904").Append(CRLF);
            sb.Append("X-MICROSOFT-CDO-APPT-SEQUENCE:0").Append(CRLF);
            sb.Append("X-MICROSOFT-CDO-ATTENDEE-CRITICAL-CHANGE:").Append(FormatUtcDateTime(DateTime.UtcNow)).Append(CRLF);
            sb.Append("X-MICROSOFT-CDO-OWNER-CRITICAL-CHANGE:").Append(FormatUtcDateTime(DateTime.UtcNow)).Append(CRLF);
            sb.Append("BEGIN:VALARM").Append(CRLF);
            sb.Append("ACTION:DISPLAY").Append(CRLF);
            sb.Append("DESCRIPTION:REMINDER").Append(CRLF);
            sb.Append("TRIGGER;RELATED=START:-PT15M").Append(CRLF);
            sb.Append("END:VALARM").Append(CRLF);
            sb.Append("END:VEVENT").Append(CRLF);
            sb.Append("END:VCALENDAR").Append(CRLF);

            return sb.ToString();
        }
    }
}
