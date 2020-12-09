s" "          base64-decode-len 0 <> throw
s" Zg=="      base64-decode-len 0 <> throw
s" Zm8="      base64-decode-len 0 <> throw
s" Zm9v"      base64-decode-len 0 <> throw
s" Zm9vYg=="  base64-decode-len 0 <> throw
s" Zm9vYmE="  base64-decode-len 0 <> throw
s" Zm9vYmFy"  base64-decode-len 0 <> throw

s" "          base64-decode s" "        test
s" Zg=="      base64-decode s" f"       test
s" Zm8="      base64-decode s" fo"      test
s" Zm9v"      base64-decode s" foo"     test
s" Zm9vYg=="  base64-decode s" foob"    test
s" Zm9vYmE="  base64-decode s" fooba"   test
s" Zm9vYmFy"  base64-decode s" foobar"  test
