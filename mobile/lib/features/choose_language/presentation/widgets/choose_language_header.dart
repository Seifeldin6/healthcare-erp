import 'package:flutter/material.dart';
import 'package:flutter_animate/flutter_animate.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/features/splash/presentation/widgets/splash_logo.dart';
import 'package:healthcare_app/generated/l10n.dart';

class ChooseLanguageHeader extends StatelessWidget {
  const ChooseLanguageHeader({super.key});

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);
    final text = Theme.of(context).textTheme;

    return Column(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        SizedBox(
          width: 104.w,
          height: 104.w,
          child: const SplashLogo(),
        ),
        SizedBox(height: 18.h),
        Text(
          s.appName,
          style: text.displayLarge?.copyWith(color: Colors.white),
        ),
        SizedBox(height: 4.h),
        Text(
          s.appNameSecondary,
          style: text.titleMedium?.copyWith(color: Colors.white),
        ),
      ],
    ).animate().fadeIn(duration: 500.ms);
  }
}