using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.JSON
{
    enum JTokenType
    {
        OPEN_CURLY,
        CLOSE_CURLY,
        OPEN_BRACKET,
        CLOSE_BRACKET,
        STRING,
        NUMBER,
        BOOL,
        COMMA,
        COLON,
        EOF
    }

    struct JToken
    {
        public JTokenType Type;
        public object Data;
        public int Line;
        public int Space;

        public JToken(JTokenType type, object data, int line, int space)
        {
            Type = type;
            Data = data;
            Line = line;
            Space = space;
        }
    }

    public static class JSON
    {
        private static Queue<JToken> _tokens = new Queue<JToken>();

        public static object ReadJSON(string source)
        {
            _tokens.Clear();
            Lexer(source);

            if (_tokens.Count <= 1) throw new Exception("Err: Empty JSON file given");

            JToken token = _tokens.Dequeue();

            if(token.Type == JTokenType.OPEN_CURLY)
            {
                return ParseObject();
            }
            else if(token.Type == JTokenType.OPEN_BRACKET)
            {
                return ParseArray();
            }
            else
            {
                throw new Exception("Err: JSON must start with object or array");
            }
        }

        private static JToken GetToken()
        {
            JToken token = _tokens.Dequeue();
            if (token.Type == JTokenType.EOF) throw new Exception($"Err: Expected token, got EOF at {token.Line}:{token.Space}");
            return token;
        }

        private static JToken GetAssertToken(JTokenType type)
        {
            JToken token = _tokens.Dequeue();
            if (token.Type != type) throw new Exception($"Err: Expected token type {type}, got {token.Type} at {token.Line}:{token.Space}.");
            return token;
        }

        private static void AssertToken(JToken token, JTokenType type)
        {
            if (token.Type != type) throw new Exception($"Err: Expected token type {type}, got {token.Type} at {token.Line}:{token.Space}.");
        }

        private static void PeekAssertToken(JTokenType type)
        {
            JToken token = _tokens.Dequeue();
            if (token.Type != type) throw new Exception($"Err: Expected token type {type}, got {token.Type} at {token.Line}:{token.Space}.");
        }

        private static object ParseObject()
        {
            JObject jObj = new JObject();
            JToken token = GetToken();
            while(_tokens.Peek().Type != JTokenType.EOF && _tokens.Peek().Type != JTokenType.CLOSE_CURLY)
            {
                AssertToken(token, JTokenType.STRING);
                string key = token.Data as string;
                if(key.Trim().Length == 0)
                {
                    throw new Exception($"Err: Expected key, got empty string at {token.Line}:{token.Space}.");
                }
                GetAssertToken(JTokenType.COLON);
                token = GetToken();
                switch(token.Type)
                {
                    case JTokenType.NUMBER:
                    case JTokenType.BOOL:
                    case JTokenType.STRING:
                        jObj.Fields.Add(key, token.Data);
                        break;

                    case JTokenType.OPEN_CURLY:
                        jObj.Fields.Add(key, ParseObject());
                        break;

                    case JTokenType.OPEN_BRACKET:
                        jObj.Fields.Add(key, ParseArray());
                        break;

                    default:
                        throw new Exception($"Err: Expected data value, got {token.Type} at {token.Line}:{token.Space}.");
                        break;
                }

                token = GetToken();
                switch(token.Type)
                {
                    case JTokenType.COMMA:
                        // Do nothing
                        token = GetAssertToken(JTokenType.STRING);
                        break;

                    case JTokenType.CLOSE_CURLY:
                        return jObj;

                    default:
                        throw new Exception($"Err: Expected comma or close curly, got {token.Type} at {token.Line}:{token.Space}.");
                        break;
                }
            }

            return jObj;
        }

        private static object ParseArray()
        {
            JArray array = new JArray();
            JToken token = GetToken();

            while (token.Type != JTokenType.EOF && token.Type != JTokenType.CLOSE_BRACKET)
            {
                switch(token.Type)
                {
                    case JTokenType.OPEN_CURLY:
                        array.Array.Add(ParseObject());
                        break;

                    case JTokenType.OPEN_BRACKET:
                        array.Array.Add(ParseArray());
                        break;

                    case JTokenType.BOOL: 
                    case JTokenType.NUMBER: 
                    case JTokenType.STRING:
                        array.Array.Add(token.Data);
                        break;

                    default:
                        throw new Exception($"Unexpected token {token.Type} for array field at {token.Line}:{token.Space}");
                        break;
                }

                token = GetToken();
                switch (token.Type)
                {
                    case JTokenType.COMMA:
                        // Do nothing
                        token = GetToken();
                        break;

                    case JTokenType.CLOSE_BRACKET:
                        return array;

                    default:
                        throw new Exception($"Err: Expected comma or close bracket, got {token.Type} at {token.Line}:{token.Space}.");
                        break;
                }
            }

            return array;
        }

        private static void Lexer(string source)
        {
            int origIndex;
            int line = 1, space = 1;

            for(int i = 0; i < source.Length; i++)
            {
                switch(source[i])
                {
                    case '\n':
                        line++;
                        space = 0;
                        break;

                    case ' ':
                    case '\r':
                    case '\t':
                        // Do nothing
                        break;

                    case '{':
                        _tokens.Enqueue(new JToken(JTokenType.OPEN_CURLY, null, line, space));
                        break;

                    case '}':
                        _tokens.Enqueue(new JToken(JTokenType.CLOSE_CURLY, null, line, space));
                        break;

                    case '[':
                        _tokens.Enqueue(new JToken(JTokenType.OPEN_BRACKET, null, line, space));
                        break;

                    case ']':
                        _tokens.Enqueue(new JToken(JTokenType.CLOSE_BRACKET, null, line, space));
                        break;

                    case ':':
                        _tokens.Enqueue(new JToken(JTokenType.COLON, null, line, space));
                        break;

                    case ',':
                        _tokens.Enqueue(new JToken(JTokenType.COMMA, null, line, space));
                        break;

                    case '\"':
                        i++;
                        origIndex = i;
                        while (i < source.Length && source[i] != '\"') { space++; i++; }
                        if( i == source.Length )
                        {
                            throw new Exception($"Err: Missing closing '\"' at {line}:{space}.");
                        }
                        else
                        {
                            _tokens.Enqueue(new JToken(JTokenType.STRING, source.Substring(origIndex, i - origIndex), line, space));
                        }
                        break;

                    default:
                        // Load number
                        if(char.IsDigit(source[i]) || source[i] == '-')
                        {
                            bool foundDecimal = false;
                            origIndex = i;
                            i++;
                            while(i < source.Length && char.IsDigit(source[i]) || source[i] == '.')
                            {
                                space++;
                                if(source[i] == '.')
                                {
                                    if (foundDecimal)
                                    {
                                        throw new Exception($"Err: Unexpected second decimal in number at {line}:{space}");
                                    }
                                    else foundDecimal = true;
                                }
                                i++;
                            }
                            _tokens.Enqueue(new JToken(JTokenType.NUMBER, Convert.ToDouble(source.Substring(origIndex, i - origIndex)), line, space));
                            i--;
                        }
                        else if(char.IsLower(source[i]) && char.IsLetter(source[i]))
                        {
                            origIndex = i;
                            i++;
                            while (char.IsLower(source[i]) && char.IsLetter(source[i])) { space++; i++; }
                            string ident = source.Substring(origIndex, i - origIndex);
                            if (ident == "true") _tokens.Enqueue(new JToken(JTokenType.BOOL, true, line, space));
                            else if (ident == "false") _tokens.Enqueue(new JToken(JTokenType.BOOL, false, line, space));
                            else throw new Exception($"Err: Unexpected ident '{ident}' at {line}:{space}");
                            i--;
                        }
                        else
                        {
                            throw new Exception($"Err: Unexpected char '{source[i]}' at {line}:{space}");
                        }
                        break;
                }

                space++;
            }
            _tokens.Enqueue(new JToken(JTokenType.EOF, null, line, space));
        }
    }
}
