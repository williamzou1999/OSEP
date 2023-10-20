import argparse
from random import randint
import re

parser = argparse.ArgumentParser()
parser.add_argument("file", help="the path of payload file, need .bin (raw)", nargs='?')
parser.add_argument("key", help="the key of XOR encode, need integer", nargs='?', default=randint(1,255))
args = parser.parse_args()

with open(args.file, "rb") as f:
    payload = f.read()

encodePayload = []
payloadFormatted = ""
for b in payload:
    bInt = int(b)
    bInt = bInt ^ int(args.key)
    encodePayload.append("{0:#0{1}x}".format(bInt, 4))

payloadLen = len(encodePayload)
encodePayload = re.sub("(.{65})", "\\1\n", ','.join(encodePayload), 0, re.DOTALL)
payloadFormatted += f"byte[] buf = new byte[{str(payloadLen)}] {{\n{encodePayload.strip()}\n}};"

print(payloadFormatted + "\n")