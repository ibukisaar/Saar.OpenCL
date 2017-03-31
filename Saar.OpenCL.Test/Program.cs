using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saar.OpenCL;
using System.Linq.Expressions;
using System.Drawing.Imaging;
using System.Drawing;

namespace Saar.OpenCL.Test {
	class Program {
		unsafe static void Main(string[] args) {
			const string clCode = @"
__constant sampler_t imageSampler = CLK_NORMALIZED_COORDS_FALSE | CLK_ADDRESS_CLAMP | CLK_FILTER_NEAREST; 

__kernel void gray(__read_only image2d_t input, __write_only image2d_t output) {
	int2 coord = (int2)(get_global_id(0), get_global_id(1));
	uint4 pixel = read_imageui(input, imageSampler, coord);
	uint temp = ((pixel.x * 38) + (pixel.y * 75) + (pixel.z * 15)) >> 7;
	write_imageui(output, coord, temp);
}

__kernel void test(__global int* input, __global int* output) {
	int i = get_global_id(0);
	output[i] = input[i];
}
";

			Bitmap image = new Bitmap(@"Z:\test.jpg");
			var data = image.LockBits(new Rectangle(Point.Empty, image.Size), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			(Size, Size) size = (image.Width, image.Height);

			var env = new Environment(clCode);
			var kernel = env["gray"];
			var input = env.Ctx.CreateImage2D(ChannelOrder.BGRA, ChannelType.UnsignedInt8, image.Width, image.Height, data.Scan0, MemFlags.ReadOnly);
			var output = env.Ctx.CreateImage2D(ChannelOrder.BGRA, ChannelType.UnsignedInt8, image.Width, image.Height, MemFlags.WriteOnly);
			kernel.SetArgs(input, output);

			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			var e = env.Cmd.Run2DRangeKernel(kernel, size);
			env.Cmd.ReadImage2DBlocking(output, size, data.Scan0, null, null, e);
			sw.Stop();
			Console.WriteLine(sw.Elapsed);

			image.UnlockBits(data);
			image.Save(@"Z:\output3.png");
			
			env.Dispose();
		}
	}
}
