import 'package:flutter/material.dart';
import 'package:flutter_animate/flutter_animate.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/generated/l10n.dart';

class SplashTexts extends StatelessWidget {
  const SplashTexts({super.key});

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);
    final text = Theme.of(context).textTheme;

    return Column(
      children: [
        SizedBox(
          height: 40.h,
          child: Center(
            child: Text(
              s.appName,
              style: text.displayLarge?.copyWith(color: Colors.white),
            ),
          ),
        ),
        SizedBox(height: 6.h),
        SizedBox(
          height: 20.h,
          child: Center(
            child: Text(
              s.appNameSecondary,
              style: text.titleMedium?.copyWith(color: Colors.white),
            ),
          ),
        ),
        SizedBox(height: 30.h),
        SizedBox(
          width: 180.w,
          child: Text(
            s.splashTagline,
            textAlign: TextAlign.center,
            style: text.bodyLarge?.copyWith(
              color: AppColors.onPrimarySoft,
              fontSize: 15.sp,
              fontWeight: FontWeight.w500,
              height: 22 / 15,
            ),
          ),
        ),
      ],
    ).animate().fadeIn(duration: 700.ms);
  }
}