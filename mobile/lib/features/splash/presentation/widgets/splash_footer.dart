import 'package:flutter/material.dart';
import 'package:flutter_animate/flutter_animate.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/generated/l10n.dart';

class SplashFooter extends StatelessWidget {
  const SplashFooter({super.key});

  Widget _dots() => Row(
    mainAxisSize: MainAxisSize.min,
    spacing: 6.w,
    children: [
      for (var i = 0; i < 3; i++)
        CircleAvatar(radius: 3.r, backgroundColor: Colors.white)
            .animate(
              delay: (i * 240).ms,
              onPlay: (c) => c.repeat(reverse: true),
            )
            .fade(begin: 0.35, end: 1, duration: 600.ms),
    ],
  );

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);
    final text = Theme.of(context).textTheme;

    return Column(
      children: [
        _dots(),
        SizedBox(height: 24.h),
        Text.rich(
          TextSpan(
            children: [
              TextSpan(
                text: s.splashFooterEn,
                style: text.labelSmall?.copyWith(
                  color: Colors.white70,
                  letterSpacing: 1.2,
                  height: 1.4,
                ),
              ),
              TextSpan(
                text: '  ·  ${s.splashFooterAr}',
                style: text.labelSmall?.copyWith(color: Colors.white70, height: 1.4),
              ),
            ],
          ),
          textAlign: TextAlign.center,
        ),
      ],
    ).animate().fadeIn(duration: 700.ms);
  }
}
