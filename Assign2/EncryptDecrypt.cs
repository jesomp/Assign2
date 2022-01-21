using NUnit.Framework;

namespace Assign2
{
    public class EncryptDecrypt
    {
        private const string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private readonly int length;
        private int stringIndex;
        private int rand;

        private RandomNumberGenerator rng;

        public EncryptDecrypt(uint encryptionKey)
        {
            length = letters.Length;
            rng = new RandomNumberGenerator(encryptionKey);
        }

        //method to encrypt file
        public char Encrypt(char letter)
        {
            bool letterMatched = SearchAndMatchChar(letter);

            //apply below algorithm to encrypt letter if matched in the letters array
            if (letterMatched)
            {
                rand = (int)(rng.Next() % length);
                int newIndex = (stringIndex + rand);

                if (newIndex >= length)
                    newIndex -= length;

                letter = letters[newIndex];
            }

            return letter;
        }

        //method to decrypt file
        public char Decrypt(char letter)
        {
            bool letterMatched = SearchAndMatchChar(letter);

            //apply below algorithm to decrypt letter if matched in the letters array
            if (letterMatched)
            {
                rand = (int)(rng.Next() % length);
                int newIndex = (stringIndex - rand);

                if (newIndex < 0)
                    newIndex += length;

                letter = letters[newIndex];
            }

            return letter;
        }

        //search for letter in the String Constant
        private bool SearchAndMatchChar(char letter)
        {
            stringIndex = 0;
            bool letterMatched = false;

            while (stringIndex < length)
            {
                //match the letter to a character in the letters array
                if (letter.Equals(letters[stringIndex]))
                {
                    letterMatched = true;
                    break;
                }
                
                stringIndex++;
            }

            return letterMatched;
        }
    }

    [TestFixture]

    public class CheckEncryptDecrypt
    {
        [Test]
        public void CheckEncrypt()
        {
            EncryptDecrypt ed = new EncryptDecrypt(3412);

            //validate encryption of each character matches
            Assert.AreEqual(ed.Encrypt('1'), 'L');
            Assert.AreEqual(ed.Encrypt('t'), 'U');

            //validate encryption of special character matches
            Assert.AreEqual(ed.Encrypt('$'), '$');
            Assert.AreEqual(ed.Encrypt('.'), '.');

            //validate encryption of space matches
            Assert.AreEqual(ed.Encrypt(' '), ' ');   
        }

        [Test]
        public void CheckDecrypt()
        {
            EncryptDecrypt ed = new EncryptDecrypt(3412);
            
            //validate decryption of each character matches
            Assert.AreEqual(ed.Decrypt('L'), '1');
            Assert.AreEqual(ed.Decrypt('U'), 't');

            //validate decryption of special character matches
            Assert.AreEqual(ed.Decrypt('$'), '$');
            Assert.AreEqual(ed.Decrypt('.'), '.');

            //validate decryption of space matches
            Assert.AreEqual(ed.Decrypt(' '), ' ');
        }
    }
}