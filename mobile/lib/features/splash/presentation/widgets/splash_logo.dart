import 'package:flutter/material.dart';
import 'package:flutter_animate/flutter_animate.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:healthcare_app/core/helpers/constants.dart';

class SplashLogo extends StatelessWidget {
  const SplashLogo({super.key});

  @override
  Widget build(BuildContext context) {
    return Align(
      alignment: Alignment.center,
      child: SizedBox(
        width: 104.w,
        height: 104.w,
        child: Stack(
          alignment: Alignment.center,
          clipBehavior: Clip.none,
          children: [
            OverflowBox(
              maxWidth: 560.w,
              maxHeight: 560.w,
              child: SvgPicture.asset(
                AppAssets.splashRipples,
                width: 560.w,
                height: 560.w,
                fit: BoxFit.contain,
              ).animate().fadeIn(duration: 700.ms),
            ),
            Image.asset(AppAssets.splashLogo, width: 104.w, height: 104.w),
          ],
        ),
      ),
    );
  }
}
