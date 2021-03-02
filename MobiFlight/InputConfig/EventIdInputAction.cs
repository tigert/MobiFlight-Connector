﻿using MobiFlight.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobiFlight.InputConfig
{
    public class EventIdInputAction : InputAction, ICloneable
    {
        const Int16 BaseOffset = 0x3110;
        const Int16 ParamOffset = 0x3114;

        public const String TYPE = "EventIdInputAction";
        public Int32 EventId;
        public String Param = "0";
        
        override public object Clone()
        {
            EventIdInputAction clone = new EventIdInputAction();
            clone.EventId = EventId;
            clone.Param = Param;

            return clone;
        }

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            String eventId = reader["eventId"];
            String param = reader["param"];

            EventId = Int32.Parse(eventId);
            Param = param;
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("type", getType());
            writer.WriteAttributeString("eventId", EventId.ToString());
            writer.WriteAttributeString("param", Param);
        }

        protected virtual String getType()
        {
            return TYPE;
        }

        public override void execute(FSUIPC.FSUIPCCacheInterface fsuipcCache, SimConnectMSFS.SimConnectCacheInterface simConnectCache, MobiFlightCacheInterface moduleCache, List<ConfigRefValue> configRefs)
        {
            String value = Param;

            List<Tuple<string, string>> replacements = new List<Tuple<string, string>>();
            foreach (ConfigRefValue item in configRefs)
            {
                Tuple<string, string> replacement = new Tuple<string, string>(item.ConfigRef.Placeholder, item.Value);
                replacements.Add(replacement);
            }

            value = Replace(value, replacements);

            fsuipcCache.setEventID(EventId, int.Parse(value));
        }
    }
}
